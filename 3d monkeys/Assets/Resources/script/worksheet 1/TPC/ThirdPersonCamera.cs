using Assets.Resources.script.worksheet_1.TPC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Resources.script.TPC
{
    public class ThirdPersonCamera : MonoBehaviour
    {
        public Transform mPlayer;
        private TPCBase mThirdPersonCamera;

        #region game constant Values
        public Vector3 mPositionOffset = new Vector3(0.0f, 2.0f, -2.5f);
        public Vector3 mAngleOffset = new Vector3(0.0f, 0.0f, 0.0f);
        [Tooltip("The damping factor to smooth the changes in position and rotation of the camera.")]
        public float mDamping = 1.0f;
        public float mMinPitch = -30.0f;
        public float mMaxPitch = 30.0f;
        public float mRotationSpeed = 50.0f;

        #endregion

        public FixedTouchField mTouchField;

        public CameraType mCameraType = CameraType.Follow_Track_Pos;
        Dictionary<CameraType, TPCBase> mThirdPersonCameraDict = new Dictionary<CameraType, TPCBase>();

        void Start()
        {

            mThirdPersonCameraDict.Add(CameraType.Track, new TPCTrack(transform, mPlayer));
            mThirdPersonCameraDict.Add(CameraType.Follow_Track_Pos, new TPCFollowTrackPosition(transform, mPlayer));
            mThirdPersonCameraDict.Add(CameraType.Follow_Track_Pos_Rot, new TPCFollowTrackPositionAndRotation(transform, mPlayer));
            mThirdPersonCameraDict.Add(CameraType.Topdown, new TPCTopDown(transform, mPlayer));

            // We instantiate and add the new third-person camera to the dictionary
            #if UNITY_STANDALONE
                mThirdPersonCameraDict.Add(CameraType.Follow_Independent, new
                TPCFollowIndependentRotation(transform, mPlayer));
            #endif
            #if UNITY_ANDROID
                mThirdPersonCameraDict.Add(CameraType.Follow_Independent, new
                TPCFollowIndependentRotation(transform, mPlayer, mTouchField));
            #endif

            UpdatingCameraType();
            UpdatingConstants();
        }

        private void Update()
        {
            UpdatingCameraType();
            UpdatingConstants();
        }

        private void UpdatingConstants()
        {
            //updating the changes from the inspector to the camera
            GameConstants.Damping = mDamping;
            GameConstants.CameraPositionOffset = mPositionOffset;
            GameConstants.CameraAngleOffset = mAngleOffset;
            GameConstants.MinPitch = mMinPitch;
            GameConstants.MaxPitch = mMaxPitch;
            GameConstants.RotationSpeed = mRotationSpeed;
        }

        private void UpdatingCameraType()
        {
            mThirdPersonCamera = mThirdPersonCameraDict[mCameraType];
        }


        void LateUpdate()
        {
            mThirdPersonCamera.Update();
        }

        public enum CameraType
        {
            Track,
            Follow_Track_Pos,
            Follow_Track_Pos_Rot,
            Topdown,
            Follow_Independent, 
        }
    }
}