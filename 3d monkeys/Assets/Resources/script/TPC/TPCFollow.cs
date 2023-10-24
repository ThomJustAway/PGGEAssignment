using System.Collections;
using UnityEngine;

namespace Assets.Resources.script.TPC
{
    public class TPCFollow : TPCBase
    {

        public TPCFollow(Transform cameraTransform, Transform playerTransform) : base (cameraTransform, playerTransform)
        {

        }


        // Update is called once per frame
        public override void Update()
        {
            // Now we calculate the camera transformed axes.
            // We do this because our camera's rotation might have changed
            // in the derived class Update implementations. Calculate the new
            // forward, up and right vectors for the camera.

            Vector3 position = CameraTransform.position;
            Matrix4x4 matrix4X4 = new Matrix4x4(new Vector4(position.x,0,0,0), new Vector4(0 , position.y, 0, 0) , new Vector4(0, 0, position.z, 0) , new Vector4(0, 0, 0, 0));

            Vector3 forward = matrix4X4 * Vector3.forward;
            Vector3 right = matrix4X4 * Vector3.right;
            Vector3 up =matrix4X4 * Vector3.up;

            // We then calculate the offset in the camera's coordinate frame.
            // For this we first calculate the targetPos

            Vector3 targetPos = mPlayerTransform.position;

            // Add the camera offset to the target position.
            // Note that we cannot just add the offset.
            // You will need to take care of the direction as well.

            Vector3 desiredPosition = targetPos + GameConstants.CameraPositionOffset;

            // Finally, we change the position of the camera,
            // not directly, but by applying Lerp.


            position = Vector3.Lerp(mCameraTransform.position,
            desiredPosition, Time.deltaTime * GameConstants.Damping);
            mCameraTransform.position = position;
        }


    }
}