using System.Collections;
using UnityEngine;

namespace Assets.Resources.script.TPC
{
    public class ThirdPersonCamera : MonoBehaviour
    {
        public Transform mPlayer;
        private TPCBase mThirdPersonCamera;
        public Vector3 mPositionOffset = new Vector3(0.0f, 2.0f, -2.5f);

        //tpc follow
        public Vector3 mAngleOffset = new Vector3(0.0f, 0.0f, 0.0f);
        [Tooltip("The damping factor to smooth the changes in position and rotation of the camera.")]
        public float mDamping = 1.0f;


        void Start()
        {
            //mThirdPersonCamera = new TPCTrack(transform, mPlayer);
            //mThirdPersonCamera = new TPCFollowTrackPosition(transform, mPlayer);
            mThirdPersonCamera = new TPCFollowTrackPositionAndRotation(transform, mPlayer);

            //mThirdPersonCamera = new TPCTopDown(transform, mPlayer);
            //setting up the constants
            UpdatingConstants();
        }

        private void Update()
        {
            UpdatingConstants();
        }

        private void UpdatingConstants()
        {
            GameConstants.Damping = mDamping;
            GameConstants.CameraPositionOffset = mPositionOffset;
            GameConstants.CameraAngleOffset = mAngleOffset;
        }


        void LateUpdate()
        {
            mThirdPersonCamera.Update();
        }
    }
}