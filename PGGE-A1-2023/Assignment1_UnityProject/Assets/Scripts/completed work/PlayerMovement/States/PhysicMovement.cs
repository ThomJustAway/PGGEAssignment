using System.Collections;
using UnityEngine;

namespace Assets.Scripts.completed_work.PlayerMovement.States
{
    public class PhysicMovement : MovementAbstractClass
    {
        public PhysicMovement(CameraType cameraType) : base(cameraType)
        {
        }

        public override void CompleteAction()
        {
            var velocity = AmyMoveMentScript.Instance.Velocity;
            characterController.Move(velocity * Time.deltaTime);
        }
    }
}