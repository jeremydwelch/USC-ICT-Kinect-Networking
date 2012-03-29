using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

public class Nite : MonoBehaviour
{
	public bool networkStarted = false;
	public Transform playerPrefab;
    Texture2D usersLblTex;
    Color[] usersMapColors;
    Rect usersMapRect;
    int usersMapSize;
    short[] usersLabelMap;
    short[] usersDepthMap;
    float[] usersHistogramMap;
	private Rect instruct = new Rect(0, 0, Screen.width, Screen.height);
	private int syncLevel = 0;
	
    public AvatarNITE[] avatars;
    public GUIText calibration_status;
    public GUIText nite_status;
	public Texture2D instructions1;
	public Texture2D instructions2;
	public Texture2D cross;
	
    HumanSenser.UserDelegate NewUser;
    HumanSenser.UserDelegate CalibrationStarted;
    HumanSenser.UserDelegate CalibrationFailed;
    HumanSenser.UserDelegate CalibrationSuccess;
    HumanSenser.UserDelegate CalibrationLost;

    List<uint> calibratingUsers;
    public Dictionary<uint, AvatarNITE> calibratedUsers;

    static public bool initiated = false;
    static public uint rc = 99999;
    static public string config_file_name = @".\\OpenNI_Kinect.xml";
    static public uint ActiveId = 0;
    static public bool drawTexture = true;

    private bool isSkeletontransferred = false;

    void OnNewUser(uint UserId)
    {
        HumanSenser.XnVector3D com = new HumanSenser.XnVector3D();
        HumanSenser.GetUserCenterOfMass(UserId, ref com);
        calibration_status.text = String.Format("[{0}] New user", UserId);
		syncLevel = 1;
	}

    void OnCalibrationStarted(uint UserId)
    {
        HumanSenser.XnVector3D com = new HumanSenser.XnVector3D();
        HumanSenser.GetUserCenterOfMass(UserId, ref com);
        calibration_status.text = String.Format("User [{0}] Calibration started", UserId);
        calibratingUsers.Add(UserId);
    }

    void OnCalibrationFailed(uint UserId)
    {
        calibration_status.text = String.Format("User [{0}] Calibration failed", UserId);
        calibratingUsers.Remove(UserId);
		syncLevel = 1;
    }

    void OnCalibrationSuccess(uint UserId)
    {
        // remove from the calibrating list
        calibratingUsers.Remove(UserId);

        // Associate this user to an unused avatar
        for (int i = 0; i < avatars.Length; i++)
        {
            if (!calibratedUsers.ContainsValue(avatars[i]))
            {
                calibratedUsers.Add(UserId, avatars[i]);
                break;
            }
        }

        // Should we stop looking for users?
        if (calibratedUsers.Count == avatars.Length)
        {
            //Debug.Log("Stopping to look for users");
            HumanSenser.StopLookingForUsers();
        }

        ActiveId = UserId;
        uint rlt = HumanSenser.SaveSkeleton(UserId);

        calibration_status.text = String.Format("[{0}] Calibration success: {1}", UserId, Marshal.PtrToStringAnsi(HumanSenser.GetStatusString(rlt)));
		drawTexture = false;
		syncLevel = 2;
    }

    void OnCalibrationLost(uint UserId)
    {
        if (isSkeletontransferred) return;

        calibration_status.text = String.Format("[{0}] Calibration lost", UserId);
		
		if (calibratedUsers.ContainsKey(UserId))
        	calibratedUsers[UserId].RotateToCalibrationPose();
        calibratedUsers.Remove(UserId);

        // Should we start looking for users?
        if (calibratedUsers.Count < avatars.Length)
        {
            calibration_status.text = "Starting to look for users";
            HumanSenser.StartLookingForUsers(NewUser, CalibrationStarted, CalibrationFailed, CalibrationSuccess, CalibrationLost);
        }
    }

    void Start()
    {
        drawTexture = true;

        if (initiated) HumanSenser.Shutdown();

        initiated = true;
		
		rc = HumanSenser.Init(new StringBuilder(config_file_name));
        if (!DeviceManagement.isUsingMSKinectSDK())
        {
            if (rc != 0)
            {
                nite_status.text = String.Format("Error initing OpenNI: {0}", Marshal.PtrToStringAnsi(HumanSenser.GetStatusString(rc)));
            }
            else
            {
                nite_status.text = String.Format(".\\OpenNI.xml is opened!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                // Init depth & label map related stuff
                usersMapSize = HumanSenser.GetDepthWidth() * HumanSenser.GetDepthHeight();
                usersLblTex = new Texture2D(HumanSenser.GetDepthWidth(), HumanSenser.GetDepthHeight());
                usersMapColors = new Color[usersMapSize];
                usersMapRect = new Rect(Screen.width - usersLblTex.width / 2, Screen.height - usersLblTex.height / 2, usersLblTex.width / 2, usersLblTex.height / 2);
                usersLabelMap = new short[usersMapSize];
                usersDepthMap = new short[usersMapSize];
                usersHistogramMap = new float[5000];

                // register user callbacks
                calibratingUsers = new List<uint>();
                calibratedUsers = new Dictionary<uint, AvatarNITE>();
                NewUser = new HumanSenser.UserDelegate(OnNewUser);
                CalibrationStarted = new HumanSenser.UserDelegate(OnCalibrationStarted);
                CalibrationFailed = new HumanSenser.UserDelegate(OnCalibrationFailed);
                CalibrationSuccess = new HumanSenser.UserDelegate(OnCalibrationSuccess);
                CalibrationLost = new HumanSenser.UserDelegate(OnCalibrationLost);
                HumanSenser.StartLookingForUsers(NewUser, CalibrationStarted, CalibrationFailed, CalibrationSuccess, CalibrationLost);
                //Debug.Log("Waiting for users to calibrate");
            }
			Debug.Log(nite_status.text);
        }
        else 
        {
            calibratedUsers = new Dictionary<uint, AvatarNITE>();
            //calibratedUsers.Add(1, avatars[0]);
			Debug.Log("Using Microsoft SDK?");
        }
    }
	
	void OnNetworkLoadedLevel () {
		
		if(Network.peerType == NetworkPeerType.Client)
				Network.Instantiate(playerPrefab,new Vector3(-5.0f,1.184f,-6.94f),transform.rotation,0);
		if(Network.peerType == NetworkPeerType.Server)
				Network.Instantiate(playerPrefab,new Vector3(5.0f,1.184f,-6.94f),transform.rotation,0);
			
				GameObject Avatar = GameObject.Find("AvatarNITE(Clone)");
				AvatarNITE aN = Avatar.GetComponent<AvatarNITE>();
		if(Network.peerType == NetworkPeerType.Client)
				aN.RelativeToTheGameObj = GameObject.Find("Anchorclient");
		if(Network.peerType == NetworkPeerType.Server)	
				aN.RelativeToTheGameObj = GameObject.Find("Anchor");
				avatars[0] = aN;
				calibratedUsers.Add(1,aN);				
				networkStarted = true;
		
	}
	
    void Update()
    {
		if(networkStarted==false)
			return;
        if (rc == 0)
        {
            // Next NITE frame
            HumanSenser.Update(false);

            // update the visual user map
            if (drawTexture && !DeviceManagement.isUsingMSKinectSDK()) UpdateUserMap();

            foreach (KeyValuePair<uint, AvatarNITE> pair in calibratedUsers)
            {
                pair.Value.UpdateAvatar(ActiveId);
            }

            if (Input.GetKeyUp(KeyCode.Alpha0)) ActiveId = 0;
            if (Input.GetKeyUp(KeyCode.Alpha1)) ActiveId = 1;
            if (Input.GetKeyUp(KeyCode.Alpha2)) ActiveId++;
            if (Input.GetKeyUp(KeyCode.T)) TransferSkeleton();
        }
    }

    void OnApplicationQuit()
    {
        //Debug.Log("Nite Shutdown");
        if (rc == 0) HumanSenser.Shutdown();
    }

    void OnGUI()
    {
		if(networkStarted==false)
			return;
        if (rc == 0)
        {
            if (!DeviceManagement.isUsingMSKinectSDK())
            {
                if (drawTexture)
                {
					GUI.DrawTexture(usersMapRect, usersLblTex);
                    if (syncLevel > 0)
                        GUI.DrawTexture(usersMapRect, cross);
                    nite_status.text = "After Calibration, Press 'T' to Transfer Skeleton ...";
                }
                else
                {
                    nite_status.text = "";
                    calibration_status.text = "";
                }
            }
        }
        else 
        {
            if (!drawTexture)
            {
                nite_status.text = "";
                calibration_status.text = "";
            }
        }

        if (!DeviceManagement.isUsingMSKinectSDK())
        {
            switch (syncLevel)
            {
                case 0:
                    GUI.DrawTexture(instruct, instructions1);
                    break;
                case 1:
                    GUI.DrawTexture(instruct, instructions2);
                    break;
                default:
                    break;
            }
        }
//		int x=0,y=200;
//		GUI.Label(new Rect(0,0,50,200),"I m Here");
//		GameObject[] games = FindObjectsOfType(typeof(GameObject)) as GameObject[];
//		foreach (GameObject go in games)
//   				{
//    				GUI.Label(new Rect(x,y,50,200),go.ToString());
//			x+=50;
//			if(x>500)
//			{
//				x=0;
//				y+=200;
//			}
//   				}
    }

    void LoadSkeleton(uint userId)
    {
        uint rlt = 0;
        rlt = HumanSenser.LoadSkeleton(userId);
        calibration_status.text = String.Format("Apply Skeleton to {0}: {1}", userId, Marshal.PtrToStringAnsi(HumanSenser.GetStatusString(rlt)));
    }

    void TransferSkeleton()
    {
        uint rlt = 0;
        rlt = HumanSenser.TransferSkeleton(ActiveId);
        calibration_status.text = String.Format("Transfer Skeleton: {0}", rlt);

        if (rlt != 0)
        {
            // remove from the calibrating list
            calibratingUsers.Remove(ActiveId);

            // Associate this user to an unused avatar
            for (int i = 0; i < avatars.Length; i++)
            {
                if (!calibratedUsers.ContainsValue(avatars[i]))
                {
                    calibratedUsers.Add(rlt, avatars[i]);
                    break;
                }
            }
            ActiveId = rlt;
        }

        isSkeletontransferred = true;
    }

    public static void Exit()
    {
        if (rc == 0) HumanSenser.Shutdown();
    }

    void UpdateUserMap()
    {
        // copy over the maps
        Marshal.Copy(HumanSenser.GetUsersLabelMap(), usersLabelMap, 0, usersMapSize);
        Marshal.Copy(HumanSenser.GetUsersDepthMap(), usersDepthMap, 0, usersMapSize);

        // we will be flipping the texture as we convert label map to color array
        int flipIndex, i;
        int numOfPoints = 0;
        Array.Clear(usersHistogramMap, 0, usersHistogramMap.Length);

        // calculate cumulative histogram for depth
        for (i = 0; i < usersMapSize; i++)
        {
            // only calculate for depth that contains users
            if (usersLabelMap[i] != 0)
            {
                usersHistogramMap[usersDepthMap[i]]++;
                numOfPoints++;
            }
        }
        if (numOfPoints > 0)
        {
            for (i = 1; i < usersHistogramMap.Length; i++)
            {
                usersHistogramMap[i] += usersHistogramMap[i - 1];
            }
            for (i = 0; i < usersHistogramMap.Length; i++)
            {
                usersHistogramMap[i] = 1.0f - (usersHistogramMap[i] / numOfPoints);
            }
        }

        // create the actual users texture based on label map and depth histogram
        for (i = 0; i < usersMapSize; i++)
        {
            flipIndex = usersMapSize - i - 1;
            if (usersLabelMap[i] == 0)
            {
                usersMapColors[flipIndex] = Color.clear;
            }
            else
            {
                // create a blending color based on the depth histogram
                Color c = new Color(usersHistogramMap[usersDepthMap[i]], usersHistogramMap[usersDepthMap[i]], usersHistogramMap[usersDepthMap[i]], 0.9f);
                switch (usersLabelMap[i] % 4)
                {
                    case 0:
                        usersMapColors[flipIndex] = Color.red * c;
                        break;
                    case 1:
                        usersMapColors[flipIndex] = Color.green * c;
                        break;
                    case 2:
                        usersMapColors[flipIndex] = Color.blue * c;
                        break;
                    case 3:
                        usersMapColors[flipIndex] = Color.magenta * c;
                        break;
                }
            }
        }

        usersLblTex.SetPixels(usersMapColors);
        usersLblTex.Apply();
    }
	
	public int getSyncLevel()
	{
		return syncLevel;
	}
}

public class UnityUtils
{
    // Recursive
    public static Transform FindTransform(GameObject parentObj, string objName)
    {
        if (parentObj == null) return null;

        foreach (Transform trans in parentObj.transform)
        {
            if (trans.name == objName)
            {
                return trans;
            }

            Transform foundTransform = FindTransform(trans.gameObject, objName);
            if (foundTransform != null)
            {
                return foundTransform;
            }
        }

        return null;
    }
}
