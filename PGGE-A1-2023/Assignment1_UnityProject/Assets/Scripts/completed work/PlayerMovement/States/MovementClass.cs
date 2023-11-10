using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.completed_work.PlayerMovement.States
{
    public class MovementClass : MovementAbstractClass
    {
        private bool mFollowCameraForward = false;


        public MovementClass(CameraType cameraType) : base(cameraType)
        {
            
        }

        public override void CompleteAction()
        {
            throw new System.NotImplementedException();
        }
    }
}