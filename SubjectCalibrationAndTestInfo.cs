using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;

public class DeviceManagement
{
    private static bool kinect = true, isOpen1321 = false;
    private static bool using_kinect_sdk = true;
    
    public static void load_device_info()
    {
        kinect = true;
        Nite.config_file_name = @".\\OpenNI_Kinect.xml";
    }

    public static void useKinect(bool e)
    {
        kinect = e;
    }

    public static bool usingKinect()
    {
        return kinect;
    }
	
	public static bool GetOpen1321()
	{
		return isOpen1321;
	}
	
	public static void SetOpen1321(bool open)
	{
		isOpen1321 = open;
	}

    public static void SetUsingMSKinectSDK(bool open)
    {
        using_kinect_sdk = open;
    }

    public static bool isUsingMSKinectSDK()
    {
        return using_kinect_sdk;
    }
}