using System.Collections;
using UnityEngine;

namespace Assets.Resources.script.TPC
{
    public class TPCTrack : TPCBase
    {
        public TPCTrack(Transform cameraTransform, Transform playerTransform): base(cameraTransform, playerTransform)
        {

        }


        public override void Update()
        {
            Vector3 targetPos = mPlayerTransform.position;
            targetPos += GameConstants.CameraPositionOffset;
            mCameraTransform.LookAt(targetPos);
        }
    }
}