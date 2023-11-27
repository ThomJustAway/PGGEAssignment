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
        private ThirdPersonCamera camera;

        public CrouchingMovement(CameraType cameraType) : base(cameraType)
        {
            normalWalkingSpeed = AmyMoveMentScript.Instance.WalkSpeed;
            crouchingSpeed =  normalWalkingSpeed * AmyMoveMentScript.Instance.CrouchingSpeedReduction;
            isCrouching = AmyMoveMentScript.Instance.CanCrouch;
            camera = GameObject.FindObjectOfType<ThirdPersonCamera>();
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
                //decrease the height of the character and position
                tempHeight = CameraConstants.CameraPositionOffset;
                HalfHeight = tempHeight;
                HalfHeight.y *= 0.5f;
                CameraConstants.CameraPositionOffset = HalfHeight;
                characterController.height /= 2;
                characterController.center /= 2;
                camera.playerHeight /= 2;
            }
            else
            {
                //make the camera and the character height go back to the same position
                CameraConstants.CameraPositionOffset = tempHeight;
                characterController.height *= 2;
                characterController.center *= 2;
                camera.playerHeight *= 2;
            }
        }
    }
}