using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PGGE;
using UnityEngine.PlayerLoop;

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
    public Transform referencePoint;
    public CameraType mCameraType { get; private set; }
    public void ChangeCameraType(CameraType cameraType)
    {
        mCameraType = cameraType;   
    }
    Dictionary<CameraType, TPCBase> mThirdPersonCameraDict = new Dictionary<CameraType, TPCBase>();

    void Start()
    {
        mCameraType = CameraType.Follow_Track_Pos_Rot;
        // Set to CameraConstants class so that other objects can use.
        SettingCameraConstants();


        //mThirdPersonCamera = new TPCTrack(transform, mPlayer);
        //mThirdPersonCamera = new TPCFollowTrackPosition(transform, mPlayer);
        //mThirdPersonCamera = new TPCFollowTrackPositionAndRotation(transform, mPlayer);
        //mThirdPersonCamera = new TPCTopDown(transform, mPlayer);

        //populate the dictionary with the instance of the camera type
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

        //setting up the camera type of the camera
        mThirdPersonCamera = mThirdPersonCameraDict[mCameraType];

    }

    private void Update()
    {
        // Update the game constant parameters every frame 
        // so that changes applied on the editor can be reflected
        SettingCameraConstants();
        mThirdPersonCamera = mThirdPersonCameraDict[mCameraType];
    }


    private void SettingCameraConstants()
    {
        CameraConstants.Damping = mDamping;
        CameraConstants.CameraPositionOffset = mPositionOffset;
        CameraConstants.CameraAngleOffset = mAngleOffset;
        CameraConstants.MinPitch = mMinPitch;
        CameraConstants.MaxPitch = mMaxPitch;
        CameraConstants.RotationSpeed = mRotationSpeed;
        CameraConstants.CameraReferencePoint = referencePoint;
    }

    void LateUpdate()
    {
        //play the camera behaviour
        mThirdPersonCamera.Update();
    }
}
