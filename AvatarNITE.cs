/**********************************************************************************************************
 *
 * AvatarNite is keeping updating the joints info from HumanSenser to Avatar Game Object
 * 
 * *********************************************************************************************************/

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;

public class AvatarNITE : MonoBehaviour
{
	private float m_jointPositionScale = 0.001f;
    private float m_jointScale = 3000f;
    public static float distanceToCamera = 1500.0f;
    Transform relativeTo;
    public GameObject RelativeToTheGameObj;

    private Transform head;
    private Transform chest;
    private Transform lshoulderSphere;
    private Transform lshoulder;
    private Transform lelbowSphere;
    private Transform lelbow;
    private Transform lhandSphere;
    private Transform lhand;
    private Transform rshoulderSphere;
    private Transform rshoulder;
    private Transform relbowSphere;
    private Transform relbow;
    private Transform rhandSphere;
    private Transform rhand;
	// lower body
	private Transform waist;
	private Transform rhip;
	private Transform rknee;
	private Transform rfoot;
	private Transform lhip;
	private Transform lknee;
	private Transform lfoot;

	private bool handTouch;
	private bool crossTouch;
	
    private Quaternion[] initialRotations;
    private Quaternion initialRoot;
	
    static public bool RightHandHitted = false;
    static public bool LeftHandHitted = false;

    static public bool Replay = false;
    static public bool StartToPlay = false;

    public void Start()
	{
		if (RelativeToTheGameObj != null) relativeTo = RelativeToTheGameObj.transform;
	}
	public void Awake()
    {
        head = UnityUtils.FindTransform(this.gameObject,"head");
        chest = UnityUtils.FindTransform(this.gameObject,"chest");
        lshoulder = UnityUtils.FindTransform(this.gameObject, "lshoulder");
        lelbow = UnityUtils.FindTransform(this.gameObject, "lelbow");
        lhand = UnityUtils.FindTransform(this.gameObject, "lhand");
        rshoulder = UnityUtils.FindTransform(this.gameObject, "rshoulder");
        relbow = UnityUtils.FindTransform(this.gameObject, "relbow");
        rhand = UnityUtils.FindTransform(this.gameObject, "rhand");
		// lower body
		waist = UnityUtils.FindTransform(this.gameObject, "waist");
        rhip = UnityUtils.FindTransform(this.gameObject, "rhip");
        rknee = UnityUtils.FindTransform(this.gameObject, "rknee");
        rfoot = UnityUtils.FindTransform(this.gameObject, "rfoot");
        lhip = UnityUtils.FindTransform(this.gameObject, "lhip");
        lknee = UnityUtils.FindTransform(this.gameObject, "lknee");
        lfoot = UnityUtils.FindTransform(this.gameObject, "lfoot");
        
    }
	
	public void getInstance()
	{
		Debug.Log("Called connectToAvatar");
	}

	//private bool m_detectionPaused = false;
	public bool AreAllBonesDetected(uint userId)
	{
		HumanSenser.SkeletonJointTransformation trans = new HumanSenser.SkeletonJointTransformation();

		if (!DetectBone(userId, HumanSenser.SkeletonJoint.TORSO_CENTER, ref trans)) return false;
		if (!DetectBone(userId, HumanSenser.SkeletonJoint.LEFT_SHOULDER, ref trans)) return false;
		if (!DetectBone(userId, HumanSenser.SkeletonJoint.LEFT_ELBOW, ref trans)) return false;
		if (!DetectBone(userId, HumanSenser.SkeletonJoint.LEFT_HAND, ref trans)) return false;
		if (!DetectBone(userId, HumanSenser.SkeletonJoint.RIGHT_SHOULDER, ref trans)) return false;
		if (!DetectBone(userId, HumanSenser.SkeletonJoint.RIGHT_ELBOW, ref trans)) return false;
		if (!DetectBone(userId, HumanSenser.SkeletonJoint.RIGHT_HAND, ref trans)) return false;
		if (!DetectBone(userId, HumanSenser.SkeletonJoint.HEAD, ref trans)) return false;
		if (!DetectBone(userId, HumanSenser.SkeletonJoint.WAIST, ref trans)) return false;
		if (!DetectBone(userId, HumanSenser.SkeletonJoint.RIGHT_HIP, ref trans)) return false;
		if (!DetectBone(userId, HumanSenser.SkeletonJoint.RIGHT_KNEE, ref trans)) return false;
		if (!DetectBone(userId, HumanSenser.SkeletonJoint.RIGHT_FOOT, ref trans)) return false;
		if (!DetectBone(userId, HumanSenser.SkeletonJoint.LEFT_HIP, ref trans)) return false;
		if (!DetectBone(userId, HumanSenser.SkeletonJoint.LEFT_KNEE, ref trans)) return false;
		if (!DetectBone(userId, HumanSenser.SkeletonJoint.LEFT_FOOT, ref trans)) return false;

		return true;
	}
	void OnGUI()
    {
		
			if(Network.peerType == NetworkPeerType.Client)
			GUI.Label(new Rect(30,50,100,100),"Client");
			
			if(Network.peerType == NetworkPeerType.Server)
			GUI.Label(new Rect(30,50,100,100),"Server");
		
	}
	
	void OnNetworkLoadedLevel () {
 		// Instantiating SpaceCraft when Network is loaded
//		Network.Instantiate(SpaceCraft, transform.position, transform.rotation, 0);
//		GameObject obj = GameObject.Find("AvatarNITE");
//		Debug.Log("Found object");
	}
	
    public void UpdateAvatar(uint userId)
    {
		if(networkView.isMine)
		{
        if (relativeTo != null) {
            transform.position = relativeTo.position;
        }

		if (!AreAllBonesDetected(userId))
		{
			return;
		}
		
		
			//GUI.Label(new Rect(300,300,100,100),"Main Dude Hoon");
		// update locations
		TransformBone(userId, HumanSenser.SkeletonJoint.HEAD, head);
		TransformBone(userId, HumanSenser.SkeletonJoint.TORSO_CENTER, chest);
		TransformBone(userId, HumanSenser.SkeletonJoint.LEFT_SHOULDER, lshoulder);
		TransformBone(userId, HumanSenser.SkeletonJoint.LEFT_ELBOW, lelbow);
		TransformBone(userId, HumanSenser.SkeletonJoint.LEFT_HAND, lhand);
		TransformBone(userId, HumanSenser.SkeletonJoint.RIGHT_SHOULDER, rshoulder);
		TransformBone(userId, HumanSenser.SkeletonJoint.RIGHT_ELBOW, relbow);
		TransformBone(userId, HumanSenser.SkeletonJoint.RIGHT_HAND, rhand);
		// lower body
		TransformBone(userId, HumanSenser.SkeletonJoint.WAIST, waist);
		TransformBone(userId, HumanSenser.SkeletonJoint.RIGHT_HIP, rhip);
		TransformBone(userId, HumanSenser.SkeletonJoint.RIGHT_KNEE, rknee);
		TransformBone(userId, HumanSenser.SkeletonJoint.RIGHT_FOOT, rfoot);
		TransformBone(userId, HumanSenser.SkeletonJoint.LEFT_HIP, lhip);
		TransformBone(userId, HumanSenser.SkeletonJoint.LEFT_KNEE, lknee);
		TransformBone(userId, HumanSenser.SkeletonJoint.LEFT_FOOT, lfoot);
		
		// update orientations
		head.transform.rotation = getHeadAngle(chest.transform.position, head.transform.position);
        chest.transform.rotation = getJointAngle(head.transform.position, chest.transform.position);
        lshoulder.transform.rotation = getJointAngle(lelbow.transform.position, lshoulder.transform.position);
        lelbow.transform.rotation = getJointAngle(lhand.transform.position, lelbow.transform.position);
        lhand.transform.rotation = lelbow.transform.rotation;
        rshoulder.transform.rotation = getJointAngle(relbow.transform.position, rshoulder.transform.position);
        relbow.transform.rotation = getJointAngle(rhand.transform.position, relbow.transform.position);
        rhand.transform.rotation = relbow.transform.rotation;

		// update scales
		float default_scale = 1.0f / (m_jointPositionScale * m_jointScale);
		float head_scale = default_scale / 1.5f;
		float chest_scale = default_scale * 3.0f;
        head.transform.localScale = new Vector3(head_scale, head_scale, head_scale);
        chest.transform.localScale = (new Vector3(chest_scale, Vector3.Distance(chest.transform.position, rshoulder.transform.position), chest_scale));
        lshoulder.transform.localScale = (new Vector3(default_scale, Vector3.Distance(lshoulder.transform.position, lelbow.transform.position), default_scale));
        lelbow.transform.localScale = (new Vector3(default_scale, Vector3.Distance(lelbow.transform.position, lhand.transform.position), default_scale));
        rshoulder.transform.localScale = (new Vector3(default_scale, Vector3.Distance(rshoulder.transform.position, relbow.transform.position), default_scale));
        relbow.transform.localScale = (new Vector3(default_scale, Vector3.Distance(relbow.transform.position, rhand.transform.position), default_scale));
		
		waist.transform.rotation = getJointAngle(chest.transform.position, waist.transform.position);
		waist.transform.Rotate(new Vector3(-45, 0, 0));
		rhip.transform.rotation = getJointAngle(rknee.transform.position, rhip.transform.position);
		rknee.transform.rotation = getJointAngle(rfoot.transform.position, rknee.transform.position);
		rfoot.transform.rotation = rknee.transform.rotation;
		lhip.transform.rotation = getJointAngle(lknee.transform.position, lhip.transform.position);
		lknee.transform.rotation = getJointAngle(lfoot.transform.position, lknee.transform.position);
		lfoot.transform.rotation = lknee.transform.rotation;
		
		waist.transform.localScale = (new Vector3(chest_scale, Vector3.Distance(waist.transform.position, chest.transform.position), chest_scale));
        lhip.transform.localScale = (new Vector3(default_scale, Vector3.Distance(lhip.transform.position, lknee.transform.position), default_scale));
		lknee.transform.localScale = (new Vector3(default_scale, Vector3.Distance(lknee.transform.position, lfoot.transform.position), default_scale));
		rhip.transform.localScale = (new Vector3(default_scale, Vector3.Distance(rhip.transform.position, rknee.transform.position), default_scale));
		rknee.transform.localScale = (new Vector3(default_scale, Vector3.Distance(rknee.transform.position, rfoot.transform.position), default_scale));
		}
    }
	
	

    public void RotateToCalibrationPose()
    {
        // Calibration pose is simply initial position with hands raised up
        /*    
            RotateToInitialPosition();
            rightElbow.rotation = Quaternion.Euler(0, -90, 90) * initialRotations[(int)HumanSenser.SkeletonJoint.RIGHT_ELBOW];
            leftElbow.rotation = Quaternion.Euler(0, 90, -90) * initialRotations[(int)HumanSenser.SkeletonJoint.LEFT_ELBOW];
        */
    }

	bool DetectBone(uint userId, HumanSenser.SkeletonJoint joint, ref HumanSenser.SkeletonJointTransformation trans)
	{
		if (GetSubjectTransformation(userId, joint, ref trans))
		{
			if (trans.pos.confidence > 0.5f)
			{
				return true;
			}
		}
		return false;
	}

    bool TransformBone(uint userId, HumanSenser.SkeletonJoint joint, Transform dest)
    {
		HumanSenser.SkeletonJointTransformation trans = new HumanSenser.SkeletonJointTransformation();
		if (DetectBone(userId, joint, ref trans) == true)
		{
			Vector3 newPos = new Vector3(trans.pos.x, trans.pos.y, (trans.pos.z -= distanceToCamera));
			if (relativeTo != null)
			{
				dest.localPosition = (newPos *= m_jointPositionScale);
			}
			else
			{
				dest.position = (newPos *= m_jointPositionScale);
			}
			return true;
		}
		return false;
    }

    Quaternion getJointAngle(Vector3 jointEndPos, Vector3 jointStartPos)
    {
        Vector3 jointVect = jointEndPos - jointStartPos;
		/*
        jointVect = Vector3.Normalize(jointVect);
        float quatAngle = Mathf.Acos(Vector3.Dot(jointVect, Vector3.up));
        Vector3 quatAxis = Vector3.Normalize(Vector3.Cross(jointVect, Vector3.up));
        quatAngle *= Mathf.Rad2Deg;
        Quaternion quat = new Quaternion();
        quat = Quaternion.AngleAxis(-quatAngle, quatAxis);

        return quat;
		*/
		return Quaternion.FromToRotation(Vector3.up, jointVect);
    }
	
	Quaternion getHeadAngle(Vector3 jointEndPos, Vector3 jointStartPos)
    {
        Vector3 jointVect = jointEndPos - jointStartPos;
		 /*
        jointVect = Vector3.Normalize(jointVect);
        float quatAngle = Mathf.Acos(Vector3.Dot(jointVect, Vector3.up));
        Vector3 quatAxis = Vector3.Normalize(Vector3.Cross(jointVect, Vector3.down));
		quatAngle *= Mathf.Rad2Deg;
        Quaternion quat = new Quaternion();
        quat = Quaternion.AngleAxis(-quatAngle, quatAxis);

        return quat;
		*/
		return Quaternion.FromToRotation(Vector3.down, jointVect);
    }

    bool GetSubjectTransformation(uint userID, HumanSenser.SkeletonJoint joint, ref HumanSenser.SkeletonJointTransformation pTransformation)
    {
        if (!Replay)
        {
			if (HumanSenser.GetTransferredJointTransformation(userID, joint, ref pTransformation))
			{
				
			}
			else
			{
				return false;
			}
        }
		return true;
    }
	
	public float GetRightElbowAngle () {
		Vector3 rightForearm = lelbow.transform.position - lhand.transform.position;
		Vector3 rightUpperArm = lelbow.transform.position - lshoulder.transform.position;
		return Vector3.Angle(rightForearm, rightUpperArm);
	}
	public float GetLeftElbowAngle () {
		Vector3 leftForearm = relbow.transform.position - rhand.transform.position;
		Vector3 leftUpperArm = relbow.transform.position - rshoulder.transform.position;
		return Vector3.Angle(leftForearm, leftUpperArm);
	}
	public float GetLeftShoulderAngle () {
		Vector3 leftUpperArm = lelbow.transform.position - lshoulder.transform.position;
		Vector3 leftBody = lelbow.transform.position - new Vector3(lelbow.transform.position.x, 0, lelbow.transform.position.z);
		return Vector3.Angle(leftBody, leftUpperArm);
	}
	public float GetRightShoulderAngle () {
		Vector3 rightUpperArm = relbow.transform.position - rshoulder.transform.position;
		Vector3 rightBody = relbow.transform.position - new Vector3(relbow.transform.position.x, 0, relbow.transform.position.z);
		return Vector3.Angle(rightBody, rightUpperArm);
	}
	public Transform GetLeftElbowPosition() {
		Transform leftElbow = relbow.transform;
		return leftElbow;
	}
	public Transform GetRightElbowPosition() {
		Transform rightElbow = lelbow.transform;
		return rightElbow;
	}
	public Vector3 GetLeftHandPosition () {
		return rhand.transform.position;
	}
	public Vector3 GetRightHandPosition () {
		return lhand.transform.position;
	}
	public Vector3 GetLeftShoulderPosition () {
		return rshoulder.transform.position;
	}
	public Vector3 GetRightShoulderPosition () {
		return lshoulder.transform.position;
	}
	public float GetLean () {
		return head.transform.position.x;
	}
	public Vector3 GetHeadPosition () {
		return head.transform.position;
	}
	public Vector3 GetChestPosition () {
		return chest.transform.position;
	}
	
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
//		
//		waist.transform.localScale = (new Vector3(chest_scale, Vector3.Distance(waist.transform.position, chest.transform.position), chest_scale));
//      lhip.transform.localScale = (new Vector3(default_scale, Vector3.Distance(lhip.transform.position, lknee.transform.position), default_scale));
//		lknee.transform.localScale = (new Vector3(default_scale, Vector3.Distance(lknee.transform.position, lfoot.transform.position), default_scale));
//		rhip.transform.localScale = (new Vector3(default_scale, Vector3.Distance(rhip.transform.position, rknee.transform.position), default_scale));
//		rknee.transform.localScale = (new Vector3(default_scale, Vector3.Distance(rknee.transform.position, rfoot.transform.position), default_scale));
        if (stream.isWriting) {
			Vector3 h = head.position;//
			stream.Serialize( ref h);
			h = chest.position;//
			stream.Serialize( ref h);
			h = lshoulder.position;//
			stream.Serialize( ref h);
			h = lelbow.position;//
			stream.Serialize( ref h);
			h = lhand.position;//
			stream.Serialize( ref h);
			h = rshoulder.position;//
			stream.Serialize( ref h);
			h = relbow.position;//
			stream.Serialize( ref h);
			h = rhand.position;//
			stream.Serialize( ref h);
			h = waist.position;//
			stream.Serialize( ref h);
			h = rhip.position;//
			stream.Serialize( ref h);
			h = rknee.position;//
			stream.Serialize( ref h);
			h = rfoot.position;//
			stream.Serialize( ref h);
			h = lhip.position;//
			stream.Serialize( ref h);
			h = lknee.position;//
			stream.Serialize( ref h);
			h = lfoot.position;//
			stream.Serialize( ref h);
			
			Quaternion r = head.transform.rotation;
			stream.Serialize( ref r);
			r = chest.transform.rotation;
			stream.Serialize( ref r);
			r = lshoulder.transform.rotation;
			stream.Serialize( ref r);
			r = lelbow.transform.rotation;
			stream.Serialize( ref r);
			r = lhand.transform.rotation;
			stream.Serialize( ref r);
			r = rshoulder.transform.rotation;
			stream.Serialize( ref r);
			r = relbow.transform.rotation;
			stream.Serialize( ref r);
			r = rhand.transform.rotation;
			stream.Serialize( ref r);
			
			h = head.transform.localScale;
			stream.Serialize( ref h);
			h = chest.transform.localScale;
			stream.Serialize( ref h);
			h = lshoulder.transform.localScale;
			stream.Serialize( ref h);
			h = lelbow.transform.localScale;
			stream.Serialize( ref h);
			h = rshoulder.transform.localScale;
			stream.Serialize( ref h);
			h = relbow.transform.localScale;
			stream.Serialize( ref h);

			
			r = waist.transform.rotation;
			stream.Serialize( ref r);
			r = rhip.transform.rotation;
			stream.Serialize( ref r);
			r = rknee.transform.rotation;
			stream.Serialize( ref r);
			r = rfoot.transform.rotation;
			stream.Serialize( ref r);
			r = lhip.transform.rotation;
			stream.Serialize( ref r);
			r = lknee.transform.rotation;
			stream.Serialize( ref r);
			r = lfoot.transform.rotation;
			stream.Serialize( ref r);
			
			
			h = waist.transform.localScale;
			stream.Serialize( ref h);
			h = lhip.transform.localScale;
			stream.Serialize( ref h); 
			h = lknee.transform.localScale;
			stream.Serialize( ref h);
			h = rhip.transform.localScale;
			stream.Serialize( ref h);
			h = rknee.transform.localScale;
			stream.Serialize( ref h);
			
			
			
        } else {
			Vector3 h = head.position;
			stream.Serialize( ref h);
			head.position = h;
			
			 h = chest.position;
			stream.Serialize( ref h);
			chest.position = h;
			
			 h = lshoulder.position;
			stream.Serialize( ref h);
			lshoulder.position = h;
			
			 h = lelbow.position;
			stream.Serialize( ref h);
			lelbow.position = h;
			
			h = lhand.position;
			stream.Serialize( ref h);
			lhand.position = h;
			
			 h = rshoulder.position;
			stream.Serialize( ref h);
			rshoulder.position = h;
			
			 h = relbow.position;
			stream.Serialize( ref h);
			relbow.position = h;
			
			 h = rhand.position;
			stream.Serialize( ref h);
			rhand.position = h;
			
			 h = waist.position;
			stream.Serialize( ref h);
			waist.position = h;
			
			 h = rhip.position;
			stream.Serialize( ref h);
			rhip.position = h;

			h = rknee.position;
			stream.Serialize( ref h);
			rknee.position = h;
			
			 h = rfoot.position;
			stream.Serialize( ref h);
			rfoot.position = h;
			
			h = lhip.position;
			stream.Serialize( ref h);
			lhip.position = h;
			
			h = lknee.position;
			stream.Serialize( ref h);
			lknee.position = h;
			
			h = lfoot.position;
			stream.Serialize( ref h);
			lfoot.position = h;
			
			Quaternion r = head.transform.rotation;
			stream.Serialize( ref r);
			head.transform.rotation = r;
			
			r = chest.transform.rotation;
			stream.Serialize( ref r);
			chest.transform.rotation = r;
			
			r = lshoulder.transform.rotation;
			stream.Serialize( ref r);
			lshoulder.transform.rotation = r;
			
			r = lelbow.transform.rotation;
			stream.Serialize( ref r);
			lelbow.transform.rotation = r;
			
			r = lhand.transform.rotation;
			stream.Serialize( ref r);
			lhand.transform.rotation = r;
			
			r = rshoulder.transform.rotation;
			stream.Serialize( ref r);
			rshoulder.transform.rotation = r;
			
			r = relbow.transform.rotation;
			stream.Serialize( ref r);
			relbow.transform.rotation = r;
			
			r = rhand.transform.rotation;
			stream.Serialize( ref r);
			rhand.transform.rotation = r;
			
			h = head.transform.localScale;
			stream.Serialize( ref h);
			head.transform.localScale = h;
			
			h = chest.transform.localScale;
			stream.Serialize( ref h);
			chest.transform.localScale = h;
			
			h = lshoulder.transform.localScale;
			stream.Serialize( ref h);
			lshoulder.transform.localScale = h;
			
			h = lelbow.transform.localScale;
			stream.Serialize( ref h);
			lelbow.transform.localScale = h;
			
			h = rshoulder.transform.localScale;
			stream.Serialize( ref h);
			rshoulder.transform.localScale = h;
			
			h = relbow.transform.localScale;
			stream.Serialize( ref h);
			relbow.transform.localScale = h;
			
			r = waist.transform.rotation;
			stream.Serialize( ref r);
			waist.transform.rotation = r;
			
			r = rhip.transform.rotation;
			stream.Serialize( ref r);
			rhip.transform.rotation = r;
			
			r = rknee.transform.rotation;
			stream.Serialize( ref r);
			rknee.transform.rotation = r;
			
			r = rfoot.transform.rotation;
			stream.Serialize( ref r);
			rfoot.transform.rotation = r;
			
			r = lhip.transform.rotation;
			stream.Serialize( ref r);
			lhip.transform.rotation = r;
			
			r = lknee.transform.rotation;
			stream.Serialize( ref r);
			lknee.transform.rotation = r;
			
			r = lfoot.transform.rotation;
			stream.Serialize( ref r);
			lfoot.transform.rotation = r;
			
			h = waist.transform.localScale;
			stream.Serialize( ref h);
			waist.transform.localScale = h;
			
			h = lhip.transform.localScale;
			stream.Serialize( ref h); 
			lhip.transform.localScale = h;
			
			h = lknee.transform.localScale;
			stream.Serialize( ref h);
			lknee.transform.localScale = h;
			
			h = rhip.transform.localScale;
			stream.Serialize( ref h);
			rhip.transform.localScale = h;
			
			h = rknee.transform.localScale;
			stream.Serialize( ref h);
			rknee.transform.localScale = h;
			

        }
    }
	
}

