using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using System.Text;

public class HumanBody
{
    static public HumanSenser.SkeletonJointTransformation head;
    static public HumanSenser.SkeletonJointTransformation chest;
    static public HumanSenser.SkeletonJointTransformation lshoulder;
    static public HumanSenser.SkeletonJointTransformation lelbow;
    static public HumanSenser.SkeletonJointTransformation lhand;
    static public HumanSenser.SkeletonJointTransformation rshoulder;
    static public HumanSenser.SkeletonJointTransformation relbow;
    static public HumanSenser.SkeletonJointTransformation rhand;

    //to store the joints' info from a file
    static public HumanSenser.SkeletonJointTransformation[] rec_head;
    static public HumanSenser.SkeletonJointTransformation[] rec_chest;
    static public HumanSenser.SkeletonJointTransformation[] rec_lshoulder;
    static public HumanSenser.SkeletonJointTransformation[] rec_lelbow;
    static public HumanSenser.SkeletonJointTransformation[] rec_lhand;
    static public HumanSenser.SkeletonJointTransformation[] rec_rshoulder;
    static public HumanSenser.SkeletonJointTransformation[] rec_relbow;
    static public HumanSenser.SkeletonJointTransformation[] rec_rhand;

    public static void reload_joint_info(HumanSenser.SkeletonJoint joint, ref HumanSenser.SkeletonJointTransformation dest, int index)
    {
        switch (joint)
        {
            case HumanSenser.SkeletonJoint.HEAD:
                dest = rec_head[index];
                break;
            case HumanSenser.SkeletonJoint.TORSO_CENTER:
                dest = rec_chest[index];
                break;
            case HumanSenser.SkeletonJoint.LEFT_SHOULDER:
                dest = rec_lshoulder[index];
                break;
            case HumanSenser.SkeletonJoint.LEFT_ELBOW:
                dest = rec_lelbow[index];
                break;
            case HumanSenser.SkeletonJoint.LEFT_HAND:
                dest = rec_lhand[index];
                break;
            case HumanSenser.SkeletonJoint.RIGHT_SHOULDER:
                dest = rec_rshoulder[index];
                break;
            case HumanSenser.SkeletonJoint.RIGHT_ELBOW:
                dest = rec_relbow[index];
                break;
            case HumanSenser.SkeletonJoint.RIGHT_HAND:
                dest = rec_rhand[index];
                break;
        }
    }
	
    public static void create_joint_array(HumanSenser.SkeletonJoint joint, int size)
    {
        switch (joint)
        {
            case HumanSenser.SkeletonJoint.HEAD:
                rec_head = new HumanSenser.SkeletonJointTransformation[size];
                break;
            case HumanSenser.SkeletonJoint.TORSO_CENTER:
                rec_chest = new HumanSenser.SkeletonJointTransformation[size];
                break;
            case HumanSenser.SkeletonJoint.LEFT_SHOULDER:
                rec_lshoulder = new HumanSenser.SkeletonJointTransformation[size];
                break;
            case HumanSenser.SkeletonJoint.LEFT_ELBOW:
                rec_lelbow = new HumanSenser.SkeletonJointTransformation[size];
                break;
            case HumanSenser.SkeletonJoint.LEFT_HAND:
                rec_lhand = new HumanSenser.SkeletonJointTransformation[size];
                break;
            case HumanSenser.SkeletonJoint.RIGHT_SHOULDER:
                rec_rshoulder = new HumanSenser.SkeletonJointTransformation[size];
                break;
            case HumanSenser.SkeletonJoint.RIGHT_ELBOW:
                rec_relbow = new HumanSenser.SkeletonJointTransformation[size];
                break;
            case HumanSenser.SkeletonJoint.RIGHT_HAND:
                rec_rhand = new HumanSenser.SkeletonJointTransformation[size];
                break;
        }
    }

    public static void copy_joint_info(HumanSenser.SkeletonJointTransformation src, ref HumanSenser.SkeletonJointTransformation dest)
    {
        dest.ori.confidence = src.ori.confidence;
        dest.ori.m00 = src.ori.m00;
        dest.ori.m01 = src.ori.m01;
        dest.ori.m02 = src.ori.m02;
        dest.ori.m10 = src.ori.m10;
        dest.ori.m11 = src.ori.m11;
        dest.ori.m12 = src.ori.m12;
        dest.ori.m20 = src.ori.m20;
        dest.ori.m21 = src.ori.m21;
        dest.ori.m22 = src.ori.m22;
        dest.pos.confidence = src.pos.confidence;
        dest.pos.x = src.pos.x;
        dest.pos.y = src.pos.y;
        dest.pos.z = src.pos.z;
    }

    public static void assign_joint_value(HumanSenser.SkeletonJointTransformation src, int joint, int index)
    {
        switch (joint) 
        {
            case 0:
                copy_joint_info(src, ref rec_head[index]);
                break;
            case 1:
                copy_joint_info(src, ref rec_chest[index]);
                break;
            case 2:
                copy_joint_info(src, ref rec_lshoulder[index]);
                break;
            case 3:
                copy_joint_info(src, ref rec_lelbow[index]);
                break;
            case 4:
                copy_joint_info(src, ref rec_lhand[index]);
                break;
            case 5:
                copy_joint_info(src, ref rec_rshoulder[index]);
                break;
            case 6:
                copy_joint_info(src, ref rec_relbow[index]);
                break;
            case 7:
                copy_joint_info(src, ref rec_rhand[index]);
                break;
        }

    }
}
