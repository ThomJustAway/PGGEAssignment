using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.Windows;

namespace Assets.Scripts.completed_work.PlayerMovement.States
{
    public class MovementClass : MovementAbstractClass
    {
        
        public MovementClass(CameraType cameraType) : base(cameraType)
        {
            
        }

        public override void CompleteAction()
        {
            if (CameraType == CameraType.Follow_Track_Pos_Rot ||
                CameraType == CameraType.Follow_Track_Pos 
                )
            {
                StandardMovement();
            }
            else if(CameraType == CameraType.Follow_Independent)
            {
                IndependentCameraMovement();
            }
        }

        private void StandardMovement()
        {
            float vInput = AmyMoveMentScript.Instance.InputVector.y;
            float hInput = AmyMoveMentScript.Instance.InputVector.x;
            float rotationSpeed = AmyMoveMentScript.Instance.RotationSpeed;
            //since the value is normalize, I have to change it back to make it not normalize
            if (vInput > 0.0f) vInput = 1.0f;
            else if (vInput < 0.0f) vInput = -1.0f;
            if (hInput > 0.0f) hInput = 1.0f;
            else if (hInput < 0.0f) hInput = -1.0f;

            transform.Rotate(0.0f, hInput * rotationSpeed * Time.deltaTime, 0.0f);

            MovingBasedOnVInputOnly(vInput);
        }
        private void IndependentCameraMovement()
        {
            float turnRate = AmyMoveMentScript.Instance.TurnRate;

            Vector3 eu = Camera.main.transform.rotation.eulerAngles;
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.Euler(0.0f, eu.y, 0.0f),
                turnRate * Time.deltaTime);
            float vInput = AmyMoveMentScript.Instance.InputVector.y;
            if (vInput > 0.0f) vInput = 1.0f;
            MovingBasedOnVInputOnly(vInput);
        }

        protected virtual void MovingBasedOnVInputOnly(float vInput)
        {
            float walkingSpeed = AmyMoveMentScript.Instance.WalkSpeed;

            Vector3 forward = transform.TransformDirection(Vector3.forward).normalized; //geting the forward of the transform from world space
            forward.y = 0.0f; //remove any upward force

            float speed = walkingSpeed;
            if (AmyMoveMentScript.Instance.Sprinting)
            {
                speed *= 2.0f;
            }

            characterController.Move(forward * vInput * speed * Time.deltaTime);

            animator.SetFloat("PosZ", vInput * speed / (2.0f * walkingSpeed));

            animator.SetFloat("PosX", 0);
        }

    }
}