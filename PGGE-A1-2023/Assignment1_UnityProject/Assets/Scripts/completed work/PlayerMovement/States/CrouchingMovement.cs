using PGGE;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.completed_work.PlayerMovement.States
{
    public class CrouchingMovement : MovementClass
    {
        private float normalWalkingSpeed;
        private float crouchingSpeed;

        private string animationName = "Crouch";
        private ThirdPersonCamera camera;

        private float initialCharacterHeight;
        private float initialCharacterCenterY;
        private Vector3 initialCameraOffset;

        public CrouchingMovement(CameraType cameraType, MovementStateManager stateManager) : base(cameraType, stateManager)
        {
            normalWalkingSpeed = AmyMoveMentScript.Instance.WalkSpeed;
            crouchingSpeed = normalWalkingSpeed * AmyMoveMentScript.Instance.CrouchingSpeedReduction;
            camera = GameObject.FindObjectOfType<ThirdPersonCamera>();

            initialCharacterCenterY = characterController.center.y;
            initialCharacterHeight = characterController.height;    
        }
        protected override void Start()
        {
            //start the crouching animation
            animator.SetBool(animationName, true);

            initialCameraOffset = CameraConstants.CameraPositionOffset; //keeping a reference of the value to be called reset it.

            //decrease the height of the character and position
            initialCameraOffset = CameraConstants.CameraPositionOffset;  //clone the offset so that it I can make changes to it
            var halfHeight = initialCameraOffset;
            halfHeight.y /= 2;
            CameraConstants.CameraPositionOffset = halfHeight; //lower the camera height by half 

            characterController.height = initialCharacterHeight / 2;
            characterController.center = new Vector3(0, initialCharacterCenterY / 2, 0); 
            //halfing the height and center of the controller to make sure it stay players can crawl 
            //without hitting object above

            //this is for the raycasting
            
        }

        protected override void MovingBasedOnVInputOnly(float vInput)
        {
            //overide this method since crouching and moving are technically the same.

            Vector3 forward = transform.TransformDirection(Vector3.forward).normalized; 
            forward.y = 0.0f; 
            characterController.Move(forward * vInput * crouchingSpeed * Time.deltaTime);

            //setting the float values in the animator to show the movement
            animator.SetFloat("PosZ", vInput / 2);
            animator.SetFloat("PosX", 0);
        }

        protected override MovementAbstractClass DecisionOfState()
        {
            RetrievingInputValues(
                out Vector2 input,
                out bool isAttacking,
                out bool isJumping,
                out bool isCrouching,
                out bool isReloading
                );

            if (isCrouching)
            {
                //if crouching
                return this;
            }
            else if(input != Vector2.zero)
            {
                //move the character if crouching is diabled
                 return stateManager.normalGroundMovement;
            }
            else
            {
                 return stateManager.idleMovement;
            }
        }

        protected override void Exit()
        {
            animator.SetBool(animationName, false); //make the player stand up

            CameraConstants.CameraPositionOffset = initialCameraOffset; //reset the camera offet

            //reset the values of the character controller and the camera
            characterController.height = initialCharacterHeight;
            characterController.center = new Vector3(0, initialCharacterCenterY , 0);

        }

    }
}