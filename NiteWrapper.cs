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


public class NiteWrapper
{
    const string DLL_Name = "UnityInterface.dll";

    [DllImport(DLL_Name)]
    public static extern uint Init(StringBuilder strXmlPath);
    [DllImport(DLL_Name)]
    public static extern void Update(bool async);
    [DllImport(DLL_Name)]
    public static extern void Shutdown();

    [DllImport(DLL_Name)]
    public static extern IntPtr GetStatusString(uint rc);
    [DllImport(DLL_Name)]
    public static extern int GetDepthWidth();
    [DllImport(DLL_Name)]
    public static extern int GetDepthHeight();
    [DllImport(DLL_Name)]
    public static extern IntPtr GetUsersLabelMap();
    [DllImport(DLL_Name)]
    public static extern IntPtr GetUsersDepthMap();
    [DllImport(DLL_Name)]
    public static extern int GetNumberOfUsers();

    [DllImport(DLL_Name)]
    public static extern void SetSkeletonSmoothing(double factor);
    [DllImport(DLL_Name)]
    public static extern bool GetJointTransformation(uint userID, HumanSenser.SkeletonJoint joint, ref HumanSenser.SkeletonJointTransformation pTransformation);
    [DllImport(DLL_Name)]
    public static extern bool GetTransferredJointTransformation(uint userID, HumanSenser.SkeletonJoint joint, ref HumanSenser.SkeletonJointTransformation pTransformation);
    [DllImport(DLL_Name)]
    public static extern uint TransferSkeleton(uint userID);
    [DllImport(DLL_Name)]
    public static extern uint SaveSkeleton(uint userID);
    [DllImport(DLL_Name)]
    public static extern uint LoadSkeleton(uint userID);


    [DllImport(DLL_Name)]
    public static extern void StartLookingForUsers(IntPtr NewUser, IntPtr CalibrationStarted, IntPtr CalibrationFailed, IntPtr CalibrationSuccess, IntPtr UserLost);
    [DllImport(DLL_Name)]
    public static extern void StopLookingForUsers();
    [DllImport(DLL_Name)]
    public static extern void LoseUsers();
    [DllImport(DLL_Name)]
    public static extern bool GetUserCenterOfMass(uint userID, ref HumanSenser.XnVector3D pCenterOfMass);
}

public class NiteWrapper1321
{
    const string DLL_Name = "UnityInterfaceNite140.dll";

    [DllImport(DLL_Name)]
    public static extern uint Init(StringBuilder strXmlPath);
    [DllImport(DLL_Name)]
    public static extern void Update(bool async);
    [DllImport(DLL_Name)]
    public static extern void Shutdown();

    [DllImport(DLL_Name)]
    public static extern IntPtr GetStatusString(uint rc);
    [DllImport(DLL_Name)]
    public static extern int GetDepthWidth();
    [DllImport(DLL_Name)]
    public static extern int GetDepthHeight();
    [DllImport(DLL_Name)]
    public static extern IntPtr GetUsersLabelMap();
    [DllImport(DLL_Name)]
    public static extern IntPtr GetUsersDepthMap();
    [DllImport(DLL_Name)]
    public static extern int GetNumberOfUsers();

    [DllImport(DLL_Name)]
    public static extern void SetSkeletonSmoothing(double factor);
    [DllImport(DLL_Name)]
    public static extern bool GetJointTransformation(uint userID, HumanSenser.SkeletonJoint joint, ref HumanSenser.SkeletonJointTransformation pTransformation);
    [DllImport(DLL_Name)]
    public static extern bool GetTransferredJointTransformation(uint userID, HumanSenser.SkeletonJoint joint, ref HumanSenser.SkeletonJointTransformation pTransformation);
    [DllImport(DLL_Name)]
    public static extern uint TransferSkeleton(uint userID);
    [DllImport(DLL_Name)]
    public static extern uint SaveSkeleton(uint userID);
    [DllImport(DLL_Name)]
    public static extern uint LoadSkeleton(uint userID);


    [DllImport(DLL_Name)]
    public static extern void StartLookingForUsers(IntPtr NewUser, IntPtr CalibrationStarted, IntPtr CalibrationFailed, IntPtr CalibrationSuccess, IntPtr UserLost);
    [DllImport(DLL_Name)]
    public static extern void StopLookingForUsers();
    [DllImport(DLL_Name)]
    public static extern void LoseUsers();
    [DllImport(DLL_Name)]
    public static extern bool GetUserCenterOfMass(uint userID, ref HumanSenser.XnVector3D pCenterOfMass);
}


