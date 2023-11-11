using PGGE;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.completed_work.PlayerMovement.States
{
    public class CrouchingMovement : MovementClass
    {
        private float normalWalkingSpeed;
        private float crouchingSpeed;
        private bool isCrouching;

        private Vector3 HalfHeight;
        private Vector3 tempHeight;
        private string animationName = "Crouch";

        public CrouchingMovement(CameraType cameraType) : base(cameraType)
        {
            normalWalkingSpeed = AmyMoveMentScript.Instance.WalkSpeed;
            crouchingSpeed =  normalWalkingSpeed * AmyMoveMentScript.Instance.CrouchingSpeedReduction;
            isCrouching = AmyMoveMentScript.Instance.CanCrouch;
        }

        protected override void MovingBasedOnVInputOnly(float vInput)
        {
            //overide this method since crouching and moving are technically the same.

            Vector3 forward = transform.TransformDirection(Vector3.forward).normalized; 
            forward.y = 0.0f; 
            characterController.Move(forward * vInput * crouchingSpeed * Time.deltaTime);

            animator.SetFloat("PosZ", vInput / 2);
            animator.SetFloat("PosX", 0);
        }

        public void ChangeCrouchingValue( bool value)
        {
            isCrouching = value;
            animator.SetBool(animationName, isCrouching);
            if (isCrouching)
            {
                tempHeight = CameraConstants.CameraPositionOffset;
                HalfHeight = tempHeight;
                HalfHeight.y *= 0.5f;
                CameraConstants.CameraPositionOffset = HalfHeight;
            }
            else
            {
                CameraConstants.CameraPositionOffset = tempHeight;
            }
        }
    }
}