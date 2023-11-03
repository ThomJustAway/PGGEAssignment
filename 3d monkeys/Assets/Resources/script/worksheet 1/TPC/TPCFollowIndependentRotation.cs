﻿using System.Collections;
using UnityEngine;

namespace Assets.Resources.script.worksheet_1.TPC
{
    public class TPCFollowIndependentRotation : TPCBase
    {
        FixedTouchField mTouchField;
        private float angleX = 0.0f;
        public TPCFollowIndependentRotation(Transform cameraTransform,
        Transform playerTransform)
        : base(cameraTransform, playerTransform)
        {
        }

        #if UNITY_ANDROID
        public TPCFollowIndependentRotation(Transform cameraTransform,
        Transform playerTransform, FixedTouchField fixedTouch)
        : base(cameraTransform, playerTransform)
        {
            mTouchField = fixedTouch;
        }
        #endif


        public override void Update()
        {
            //implement the Update for this camera controls

            #if UNITY_STANDALONE
            float mx, my;
                mx = Input.GetAxis("Mouse X");
                my = Input.GetAxis("Mouse Y");
            #endif
            #if UNITY_ANDROID
                float mx, my;
                mx = mTouchField.TouchDist.x * Time.deltaTime;
                my = mTouchField.TouchDist.y * Time.deltaTime;
#endif

            // We apply the initial rotation to the camera.
            Quaternion initialRotation =
            Quaternion.Euler(GameConstants.CameraAngleOffset);

            Vector3 eu = mCameraTransform.rotation.eulerAngles;
            angleX -= my * GameConstants.RotationSpeed;

            // We clamp the angle along the Xaxis to be between the min and max pitch.
            angleX = Mathf.Clamp(angleX, GameConstants.MinPitch,
            GameConstants.MaxPitch);
            eu.y += mx * GameConstants.RotationSpeed;

            Quaternion newRot = Quaternion.Euler(angleX, eu.y, 0.0f) *
            initialRotation;
            mCameraTransform.rotation = newRot;


            Vector3 forward = mCameraTransform.rotation * Vector3.forward;
            Vector3 right = mCameraTransform.rotation * Vector3.right;
            Vector3 up = mCameraTransform.rotation * Vector3.up;

            Vector3 targetPos = mPlayerTransform.position;

            Vector3 desiredPosition = targetPos
            + forward * GameConstants.CameraPositionOffset.z
            + right * GameConstants.CameraPositionOffset.x
            + up * GameConstants.CameraPositionOffset.y;

            Vector3 position = Vector3.Lerp(mCameraTransform.position,
            desiredPosition,
            Time.deltaTime * GameConstants.Damping);

            mCameraTransform.position = position;
        }
    }
}