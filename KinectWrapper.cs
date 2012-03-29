/*
 * KinectWrapper.cs - Handles the connection to the mircosoft kinect sdk through
 * 			the plugin.
 * 
 * 		Developed by Peter Kinney -- 6/30/2011
 * 
 */

using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

//Assignments for a bitmask to control which bones to look at and which to ignore
public enum BoneMask
{
	None = 0x0,
	Hip_Center = 0x1,
	Spine = 0x2,
	Shoulder_Center = 0x4,
	Head = 0x8,
	Shoulder_Left = 0x10,
	Elbow_Left = 0x20,
	Wrist_Left = 0x40,
	Hand_Left = 0x80,
	Shoulder_Right = 0x100,
	Elbow_Right = 0x200,
	Wrist_Right = 0x400,
	Hand_Right = 0x800,
	Hip_Left = 0x1000,
	Knee_Left = 0x2000,
	Ankle_Left = 0x4000,
	Foot_Left = 0x8000,
	Hip_Right = 0x10000,
	Knee_Right = 0x20000,
	Ankle_Right = 0x40000,
	Foot_Right = 0x80000,
	All = 0xFFFFF,
	Right_Arm = 0x100F00,
	R_Arm_Chest = 0x100F02,
	Left_Arm = 0x1000F0
}

//Ties the appropriate bone name to each index
public enum BoneIndex
{
	Hip_Center,	Spine, Shoulder_Center, Head,
	Shoulder_Left,Elbow_Left, Wrist_Left, Hand_Left,
	Shoulder_Right, Elbow_Right, Wrist_Right,	Hand_Right,
	Hip_Left, Knee_Left,	Ankle_Left,	Foot_Left,
	Hip_Right, Knee_Right, Ankle_Right, Foot_Right,
	Num_Bones
}


public class KinectWrapper : MonoBehaviour
{	
	//variables to control how the wrapper works
	static public bool facingCamera = false;
    static public bool autoCalibrate = false;
    static public float manualCalibrate = 0.9f;
	
	//variables for other scripts to use
    static public Vector4[] BonePos;
    static public Vector4[] BoneVel;
    static public Vector4 nullVector;
	
	//For each bone, what global vector should it use as the starting point of it's up vector
	public static Vector3[] BoneBaseUp = {
		Vector3.right,Vector3.right, Vector3.zero, Vector3.zero,
		-Vector3.forward,-Vector3.up,-Vector3.forward,Vector3.zero,
		Vector3.forward,Vector3.up,Vector3.forward,Vector3.zero,
		-Vector3.forward,-Vector3.right,Vector3.right,Vector3.zero,
		Vector3.forward,-Vector3.right,Vector3.right,Vector3.zero
	};

	//For each bone, which other bone should it base it's secondary rotation (correcting the up vector) on
	public static int[] PrevBoneList = {
		-1,-1,1,-1,
		1,4,-1,-1,
		1,8,-1,-1,
		0,-1,-1,-1,
		0,-1,-1,-1
	};
	
	//Lets make our calls from the Plugin
	[DllImport ("UnityKinectPlugin.dll")]
	private static extern bool startKinect();
	
	[DllImport ("UnityKinectPlugin.dll")]
	private static extern void setKinectAngle(long angle);
	
	[DllImport ("UnityKinectPlugin.dll")]
	private static extern void stopKinect();
	
	[DllImport ("UnityKinectPlugin.dll")]
	private static extern bool updateFrame();
	
	[DllImport ("UnityKinectPlugin.dll")]
	private static extern Vector4 getSkeleton(int bone);

    [DllImport("UnityKinectPlugin.dll")]
    private static extern Vector4 getSkeletonOf(int index, int bone);
	
	[DllImport ("UnityKinectPlugin.dll")]
	private static extern Vector4 getVelocity(int bone);
	
	[DllImport ("UnityKinectPlugin.dll")]
	private static extern Vector4 getNormal();
	
	[DllImport ("UnityKinectPlugin.dll")]
	private static extern float getHeight();
	
	//private Vector3 _normal;
	//private float _mirrorPlane;
    static private float _kinectHeight;
    static private float _kinectDistance = 2.0f;
    static private long _kinectAngle;

    static private Matrix4x4 _kinectToWorld;
	
	static public void init () {
		//start the kinect and get it's height off the ground
		if(!startKinect()){
			Debug.Log("Kinect Initialization Failed");
		}
		if(autoCalibrate){
			_kinectHeight = getHeight();
		}else{
			_kinectHeight = manualCalibrate;
		}
		//print(_kinectHeight);
		
		//determine what angle the kinect should be at, and set it
        double theta = Math.Atan((1 - _kinectHeight) / _kinectDistance);
		_kinectAngle = (long)(theta * (180 / Math.PI));
		setKinectAngle(_kinectAngle);
        //setKinectAngle(15);
		
		//create the transform matrix that converts from kinect-space to world-space
		Matrix4x4 trans = new Matrix4x4();
		trans.SetTRS( new Vector3(0,_kinectHeight, -2), Quaternion.identity, Vector3.one );
		Matrix4x4 rot = new Matrix4x4();
		Quaternion quat = new Quaternion();
		quat.eulerAngles = new Vector3(-_kinectAngle, 0, 0);
		rot.SetTRS( Vector3.zero, quat, Vector3.one );
		Matrix4x4 flip = Matrix4x4.identity;
		if(facingCamera){
			flip[0,0] = -1;
		}
		flip[2,2] = -1;
		//final transform matrix offsets the rotation of the kinect, translates to a new center, and flips the z axis
		_kinectToWorld = flip*trans*rot;
		
		//set the public nullVector equal to the zero Vector4 processed by the translation matrix
		//this value is used by other functions to tell when they are getting bad data from the kinect
		nullVector = new Vector4(0,0,0,1);
		nullVector = _kinectToWorld.MultiplyPoint3x4(nullVector);
		
		//initialize the array of bone positions and velocities
		BonePos = new Vector4[(int)BoneIndex.Num_Bones];
		BoneVel = new Vector4[(int)BoneIndex.Num_Bones];
	}
	
	void Update() {
		
	}

    static public void pollKinect()
    {
		if(updateFrame()){
			for(int ii = 0; ii < (int)BoneIndex.Num_Bones; ii++){
				BonePos[ii] = getBonePos(ii);
				// send data across
				//networkView.RPC("updateBonePos", data);
				BoneVel[ii] = getBoneVel(ii);
				// Send data across
			}
		}
	}

    static public Vector4 getBonePos(int index)
    {
		if(facingCamera && index >= (int)BoneIndex.Shoulder_Left){
			//if the data needs to be mirrored and the index is either a left or right side bone,
			//add 4 to left side bones and subtract 4 from right side bones
			//(odd groupings of 4 is left, even groupings of 4 are right side)
			int plusMinus = ((index / 4) % 2) * 2 - 1;
			Debug.Log(index);
			index += 4 * plusMinus;
			Debug.Log(index);
		}
		Vector4 pos;
		pos = getSkeleton(index);
		pos = _kinectToWorld.MultiplyPoint3x4(pos);
        //pos.y = pos.y;
		return pos;
	}

    //userID is validated from 0 to 5
    static public Vector4 getBonePosOf(int user_id, int index)
    {
        if (facingCamera && index >= (int)BoneIndex.Shoulder_Left)
        {
            //if the data needs to be mirrored and the index is either a left or right side bone,
            //add 4 to left side bones and subtract 4 from right side bones
            //(odd groupings of 4 is left, even groupings of 4 are right side)
            int plusMinus = ((index / 4) % 2) * 2 - 1;
            Debug.Log(index);
            index += 4 * plusMinus;
            Debug.Log(index);
        }
        Vector4 pos;
        pos = getSkeletonOf(user_id, index);
        pos = _kinectToWorld.MultiplyPoint3x4(pos);
        //pos.y = pos.y;
        return pos;
    }

    static public Vector3 getBoneVel(int index)
    {
		Vector3 vel;
		vel = getVelocity(index);
		vel = _kinectToWorld.MultiplyVector(vel);
		return vel;
	}

    static public void OnApplicationQuit()
    {
        setKinectAngle(0);
		stopKinect();
	}
}
