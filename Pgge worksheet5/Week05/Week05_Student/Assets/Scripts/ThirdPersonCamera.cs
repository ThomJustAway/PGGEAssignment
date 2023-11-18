﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraConstant
{
    public static Vector3 CameraAngleOffset { get; set; }
    public static Vector3 CameraPositionOffset { get; set; }
    public static float Damping { get; set; }
    public static float RotationSpeed { get; set; }
    public static float MinPitch { get; set; }
    public static float MaxPitch { get; set; }

   

}


public enum CameraType
{
    Track,
    Follow_Track_Pos,
    Follow_Track_Pos_Rot,
    Topdown,
    Follow_Independent,
}


public class ThirdPersonCamera : MonoBehaviour
{
    public Transform mPlayer;

    TPCBase mThirdPersonCamera;
    // Get from Unity Editor.
    public Vector3 mPositionOffset = new Vector3(0.0f, 2.0f, -2.5f);
    public Vector3 mAngleOffset = new Vector3(0.0f, 0.0f, 0.0f);
    [Tooltip("The damping factor to smooth the changes in position and rotation of the camera.")]
    public float mDamping = 1.0f;

    public float mMinPitch = -30.0f;
    public float mMaxPitch = 30.0f;
    public float mRotationSpeed = 50.0f;
    public FixedTouchField mTouchField;

    public CameraType mCameraType = CameraType.Follow_Track_Pos;
    Dictionary<CameraType, TPCBase> mThirdPersonCameraDict = new Dictionary<CameraType, TPCBase>();

    void Start()
    {
        // Set to GameConstants class so that other objects can use.
        CameraConstant.Damping = mDamping;
        CameraConstant.CameraPositionOffset = mPositionOffset;
        CameraConstant.CameraAngleOffset = mAngleOffset;
        CameraConstant.MinPitch = mMinPitch;
        CameraConstant.MaxPitch = mMaxPitch;
        CameraConstant.RotationSpeed = mRotationSpeed;


        //mThirdPersonCamera = new TPCTrack(transform, mPlayer);
        //mThirdPersonCamera = new TPCFollowTrackPosition(transform, mPlayer);
        //mThirdPersonCamera = new TPCFollowTrackPositionAndRotation(transform, mPlayer);
        //mThirdPersonCamera = new TPCTopDown(transform, mPlayer);

        mThirdPersonCameraDict.Add(CameraType.Track, new TPCTrack(transform, mPlayer));
        mThirdPersonCameraDict.Add(CameraType.Follow_Track_Pos, new TPCFollowTrackPosition(transform, mPlayer));
        mThirdPersonCameraDict.Add(CameraType.Follow_Track_Pos_Rot, new TPCFollowTrackPositionAndRotation(transform, mPlayer));
        mThirdPersonCameraDict.Add(CameraType.Topdown, new TPCTopDown(transform, mPlayer));


        // We instantiate and add the new third-person camera to the dictionary
#if UNITY_STANDALONE
        mThirdPersonCameraDict.Add(CameraType.Follow_Independent, new TPCFollowIndependentRotation(transform, mPlayer));
#endif
#if UNITY_ANDROID
        mThirdPersonCameraDict.Add(CameraType.Follow_Independent, new TPCFollowIndependentRotation(transform, mPlayer, mTouchField));
#endif

        mThirdPersonCamera = mThirdPersonCameraDict[mCameraType];

    }

    private void Update()
    {
        // Update the game constant parameters every frame 
        // so that changes applied on the editor can be reflected
        CameraConstant.Damping = mDamping;
        CameraConstant.CameraPositionOffset = mPositionOffset;
        CameraConstant.CameraAngleOffset = mAngleOffset;
        CameraConstant.MinPitch = mMinPitch;
        CameraConstant.MaxPitch = mMaxPitch;
        CameraConstant.RotationSpeed = mRotationSpeed;

        mThirdPersonCamera = mThirdPersonCameraDict[mCameraType];
    }

    void LateUpdate()
    {
        mThirdPersonCamera.Update();
    }
}