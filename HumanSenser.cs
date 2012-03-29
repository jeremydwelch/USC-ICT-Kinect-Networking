/*******************************************************************************
 * 
 * Calibration here is for the camera calibration, not for the patient
 * 
 *******************************************************************************/

using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;


public class HumanSenser
{
    //public static KinectWrapper KinectBridge;

    public enum SkeletonJoint
    {
        NONE = 0,
        HEAD = 1,
        NECK = 2,
        TORSO_CENTER = 3,
        WAIST = 4,

        LEFT_COLLAR = 5,
        LEFT_SHOULDER = 6,
        LEFT_ELBOW = 7,
        LEFT_WRIST = 8,
        LEFT_HAND = 9,
        LEFT_FINGERTIP = 10,

        RIGHT_COLLAR = 11,
        RIGHT_SHOULDER = 12,
        RIGHT_ELBOW = 13,
        RIGHT_WRIST = 14,
        RIGHT_HAND = 15,
        RIGHT_FINGERTIP = 16,

        LEFT_HIP = 17,
        LEFT_KNEE = 18,
        LEFT_ANKLE = 19,
        LEFT_FOOT = 20,

        RIGHT_HIP = 21,
        RIGHT_KNEE = 22,
        RIGHT_ANKLE = 23,
        RIGHT_FOOT = 24,

        END
    };

    const string DLL_Name = "UnityInterfaceICT.dll"; 

    [StructLayout(LayoutKind.Sequential)]
    public struct SkeletonJointPosition
    {
        public float x, y, z;
        public float confidence;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SkeletonJointOrientation
    {
        public float m00, m01, m02,
                        m10, m11, m12,
                        m20, m21, m22;
        public float confidence;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SkeletonJointTransformation
    {
        public SkeletonJointPosition pos;
        public SkeletonJointOrientation ori;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct XnVector3D
    {
        public float x, y, z;
    }

    public static uint Init(StringBuilder strXmlPath)
    {
        if (DeviceManagement.isUsingMSKinectSDK())
        {
            KinectWrapper.init();
            return 0;
        }
        else
        {
            if (DeviceManagement.GetOpen1321())
                return NiteWrapper1321.Init(strXmlPath);
            else
                return NiteWrapper.Init(strXmlPath);
        }
    }

    public static void Update(bool async) 
    {
        if (DeviceManagement.isUsingMSKinectSDK())
        {
            KinectWrapper.pollKinect();
        }
        else
        {
            if (DeviceManagement.GetOpen1321())
                NiteWrapper1321.Update(async);
            else
                NiteWrapper.Update(async);
        }
    }

    public static void Shutdown()
    {
        if (DeviceManagement.isUsingMSKinectSDK())
        {
            KinectWrapper.OnApplicationQuit();
        }
        else
        {
            if (DeviceManagement.GetOpen1321())
                NiteWrapper1321.Shutdown();
            else
                NiteWrapper.Shutdown();
        }
    }

    public static IntPtr GetStatusString(uint rc)
    {
        if (DeviceManagement.GetOpen1321())
            return NiteWrapper1321.GetStatusString(rc);
        else
            return NiteWrapper.GetStatusString(rc);
    }

    public static int GetDepthWidth()
    {
        if (DeviceManagement.GetOpen1321())
            return NiteWrapper1321.GetDepthWidth();
        else
            return NiteWrapper.GetDepthWidth();
    }

    public static int GetDepthHeight()
    {
        if (DeviceManagement.GetOpen1321())
            return NiteWrapper1321.GetDepthHeight();
        else
            return NiteWrapper.GetDepthHeight();
    }

    public static IntPtr GetUsersLabelMap()
    {
        if (DeviceManagement.GetOpen1321())
            return NiteWrapper1321.GetUsersLabelMap();
        else
            return NiteWrapper.GetUsersLabelMap();
    }

    public static IntPtr GetUsersDepthMap()
    {
        if (DeviceManagement.GetOpen1321())
            return NiteWrapper1321.GetUsersDepthMap();
        else
            return NiteWrapper.GetUsersDepthMap();
    }

    public static int GetNumberOfUsers()
    {
        if (DeviceManagement.GetOpen1321())
            return NiteWrapper1321.GetNumberOfUsers();
        else
            return NiteWrapper.GetNumberOfUsers();
    }

    public static void SetSkeletonSmoothing(double factor)
    {
        if (DeviceManagement.GetOpen1321())
            NiteWrapper1321.SetSkeletonSmoothing(factor);
        else
            NiteWrapper.SetSkeletonSmoothing(factor);
    }

    public static bool GetJointTransformation(uint userID, SkeletonJoint joint, ref SkeletonJointTransformation pTransformation)
    {
        if (DeviceManagement.GetOpen1321())
            return NiteWrapper1321.GetJointTransformation(userID, joint, ref pTransformation);
        else
            return NiteWrapper.GetJointTransformation(userID, joint, ref pTransformation);
    }

    public static bool GetTransferredJointTransformation(uint userID, SkeletonJoint joint, ref SkeletonJointTransformation pTransformation)
    {
        int index = 0;
        if (DeviceManagement.isUsingMSKinectSDK())
        {
            switch (joint)
            {
                case HumanSenser.SkeletonJoint.HEAD:
                    index = (int)BoneIndex.Head;
                    break;
                case HumanSenser.SkeletonJoint.LEFT_ELBOW:
                    index = (int)BoneIndex.Elbow_Right;
                    break;
                case HumanSenser.SkeletonJoint.LEFT_HAND:
                    index = (int)BoneIndex.Hand_Right;
                    break;
                case HumanSenser.SkeletonJoint.LEFT_SHOULDER:
                    index = (int)BoneIndex.Shoulder_Right;
                    break;
                case HumanSenser.SkeletonJoint.TORSO_CENTER:
                    index = (int)BoneIndex.Spine;
                    break;
                case HumanSenser.SkeletonJoint.RIGHT_ELBOW:
                    index = (int)BoneIndex.Elbow_Left;
                    break;
                case HumanSenser.SkeletonJoint.RIGHT_HAND:
                    index = (int)BoneIndex.Hand_Left;
                    break;
                case HumanSenser.SkeletonJoint.RIGHT_SHOULDER:
                    index = (int)BoneIndex.Shoulder_Left;
                    break;
                case HumanSenser.SkeletonJoint.WAIST:
                    index = (int)BoneIndex.Hip_Center;
                    break;
                case HumanSenser.SkeletonJoint.RIGHT_HIP:
                    index = (int)BoneIndex.Hip_Left;
                    break;
                case HumanSenser.SkeletonJoint.RIGHT_KNEE:
                    index = (int)BoneIndex.Knee_Left;
                    break;
                case HumanSenser.SkeletonJoint.RIGHT_FOOT:
                    index = (int)BoneIndex.Foot_Left;
                    break;
                case HumanSenser.SkeletonJoint.LEFT_HIP:
                    index = (int)BoneIndex.Hip_Right;
                    break;
                case HumanSenser.SkeletonJoint.LEFT_KNEE:
                    index = (int)BoneIndex.Knee_Right;
                    break;
                case HumanSenser.SkeletonJoint.LEFT_FOOT:
                    index = (int)BoneIndex.Foot_Right;
                    break;
            }

            //userID is validated from 0 to 5
            if (userID > 0)
            {
                pTransformation.pos.x = KinectWrapper.getBonePosOf((int)userID, index).x * (-1000.0f);
                pTransformation.pos.y = KinectWrapper.getBonePosOf((int)userID, index).y * 1000.0f - 1000.0f;
                pTransformation.pos.z = KinectWrapper.getBonePosOf((int)userID, index).z * (-1000.0f);
                pTransformation.pos.confidence = 1.0f;
                pTransformation.ori.confidence = 1.0f;
                if (KinectWrapper.getBonePosOf((int)userID, (int)BoneIndex.Head).x == 0 &&
                    KinectWrapper.getBonePosOf((int)userID, (int)BoneIndex.Head).y == 0 &&
                    KinectWrapper.getBonePosOf((int)userID, (int)BoneIndex.Head).z == 0 &&
                    KinectWrapper.getBonePosOf((int)userID, index).x == 0 &&
                    KinectWrapper.getBonePosOf((int)userID, index).y == 0 &&
                    KinectWrapper.getBonePosOf((int)userID, index).z == 0)
                {
                    pTransformation.pos.confidence = 0.0f;
                    pTransformation.ori.confidence = 0.0f;
                }
            }else 
            {
                pTransformation.pos.x = KinectWrapper.getBonePos(index).x * (-1000.0f);
                pTransformation.pos.y = KinectWrapper.getBonePos(index).y * 1000.0f - 1000.0f;
                pTransformation.pos.z = KinectWrapper.getBonePos(index).z * (-1000.0f);
                pTransformation.pos.confidence = 1.0f;
                pTransformation.ori.confidence = 1.0f;
                if (KinectWrapper.getBonePos(index).x == 0 &&
                    KinectWrapper.getBonePos(index).y == 0 &&
                    KinectWrapper.getBonePos(index).z == 0 &&
                    KinectWrapper.getBonePos(index).x == 0 &&
                    KinectWrapper.getBonePos(index).y == 0 &&
                    KinectWrapper.getBonePos(index).z == 0)
                {
                    pTransformation.pos.confidence = 0.0f;
                    pTransformation.ori.confidence = 0.0f;
                }
            }
            pTransformation.ori.m00 = 0.0f;
            pTransformation.ori.m01 = 0.0f;
            pTransformation.ori.m02 = 0.0f;
            pTransformation.ori.m10 = 0.0f;
            pTransformation.ori.m11 = 0.0f;
            pTransformation.ori.m12 = 0.0f;
            pTransformation.ori.m20 = 0.0f;
            pTransformation.ori.m21 = 0.0f;
            pTransformation.ori.m22 = 0.0f;
            //if (joint == HumanSenser.SkeletonJoint.RIGHT_HAND) Debug.Log(String.Format("Right Hand {0}", KinectWrapper.getBonePos(index)));
            
            return true;
        }
        else
        {
            if (DeviceManagement.GetOpen1321())
                return NiteWrapper1321.GetTransferredJointTransformation(userID, joint, ref pTransformation);
            else
                return NiteWrapper.GetTransferredJointTransformation(userID, joint, ref pTransformation);
        }
    }

    public static uint TransferSkeleton(uint userID)
    {
        if (DeviceManagement.GetOpen1321())
            return NiteWrapper1321.TransferSkeleton(userID);
        else
            return NiteWrapper.TransferSkeleton(userID);
    }

    public static uint SaveSkeleton(uint userID)
    {
        if (DeviceManagement.GetOpen1321())
            return NiteWrapper1321.SaveSkeleton(userID);
        else
            return NiteWrapper.SaveSkeleton(userID);
    }

    public static uint LoadSkeleton(uint userID)
    {
        if (DeviceManagement.GetOpen1321())
            return NiteWrapper1321.LoadSkeleton(userID);
        else
            return NiteWrapper.LoadSkeleton(userID);
    }

    public static void StartLookingForUsers(IntPtr NewUser, IntPtr CalibrationStarted, IntPtr CalibrationFailed, IntPtr CalibrationSuccess, IntPtr UserLost)
    {
        if (DeviceManagement.GetOpen1321())
            NiteWrapper1321.StartLookingForUsers(NewUser, CalibrationStarted, CalibrationFailed, CalibrationSuccess, UserLost);
        else
            NiteWrapper.StartLookingForUsers(NewUser, CalibrationStarted, CalibrationFailed, CalibrationSuccess, UserLost);
    }

    public static void StopLookingForUsers()
    {
        if (DeviceManagement.GetOpen1321())
            NiteWrapper1321.StopLookingForUsers();
        else
            NiteWrapper.StopLookingForUsers();
    }

    public static void LoseUsers()
    {
        if (DeviceManagement.GetOpen1321())
            NiteWrapper1321.LoseUsers();
        else
            NiteWrapper.LoseUsers();
    }

    public static bool GetUserCenterOfMass(uint userID, ref XnVector3D pCenterOfMass)
    {
        if (DeviceManagement.GetOpen1321())
            return NiteWrapper1321.GetUserCenterOfMass(userID, ref pCenterOfMass);
        else
            return NiteWrapper.GetUserCenterOfMass(userID, ref pCenterOfMass);
    }

    public delegate void UserDelegate(uint userId);

    public static void StartLookingForUsers(UserDelegate NewUser, UserDelegate CalibrationStarted, UserDelegate CalibrationFailed, UserDelegate CalibrationSuccess, UserDelegate CalibrationLost)
    {
        StartLookingForUsers(
            Marshal.GetFunctionPointerForDelegate(NewUser),
            Marshal.GetFunctionPointerForDelegate(CalibrationStarted),
            Marshal.GetFunctionPointerForDelegate(CalibrationFailed),
            Marshal.GetFunctionPointerForDelegate(CalibrationSuccess),
            Marshal.GetFunctionPointerForDelegate(CalibrationLost));
    }
}

