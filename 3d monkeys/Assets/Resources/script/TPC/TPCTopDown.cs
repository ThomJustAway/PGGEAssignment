using System.Collections;
using UnityEngine;

namespace Assets.Resources.script.TPC
{
    public class TPCTopDown : TPCBase
    {

        public TPCTopDown(Transform cameraTransform , Transform playerTransform) :base(cameraTransform , playerTransform)
        {
        }

        // Update is called once per frame
        public override void Update()
        {
            Vector3 desiredPosition = PlayerTransform.transform.position;
            desiredPosition.y = GameConstants.CameraPositionOffset.y;

            Vector3 calculatedPosition = Vector3.Lerp(CameraTransform.position,
                desiredPosition, Time.deltaTime * GameConstants.Damping);
            CameraTransform.position = calculatedPosition;
        }
    }
}