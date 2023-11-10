using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.Windows;

namespace Assets.Scripts.completed_work.PlayerMovement.States
{
    public class MovementClass : MovementAbstractClass
    {
        private bool mFollowCameraForward = false;
        private Transform transform;
        private Animator animator;
        private AmyMoveMentScript player;
        private CharacterController characterController;
        public MovementClass(CameraType cameraType) : base(cameraType)
        {
            player = AmyMoveMentScript.Instance;
            transform = player.transform;
            animator = player.Animator;
            characterController = player.CharacterController;
        }

        public override void CompleteAction()
        {
            //if(CameraType == CameraType.Follow_Track_Pos_Rot)
            //{

            //}
            MovementForFollowTrackPosAndRot();
        }

        private void MovementForFollowTrackPosAndRot()
        {


            Vector3 forward = transform.TransformDirection(Vector3.forward).normalized; //geting the forward of the transform from world space
            forward.y = 0.0f; //remove any upward force

            float vInput = AmyMoveMentScript.Instance.inputVector.y;
            float speed = AmyMoveMentScript.Instance.WalkSpeed;
            float hInput = AmyMoveMentScript.Instance.inputVector.x;
            float rotationSpeed = AmyMoveMentScript.Instance.RotationSpeed;

            //since the value is normalize, I have to change it back to make it not normalize
            if(vInput > 0.0f)vInput = 1.0f;
            else if(vInput < 0.0f)vInput = -1.0f;
            if(hInput > 0.0f) hInput = 1.0f;
            else if(hInput < 0.0f) hInput = -1.0f;

            transform.Rotate(0.0f, hInput * rotationSpeed * Time.deltaTime, 0.0f);
            characterController.Move(forward * vInput * speed * Time.deltaTime);

            animator.SetFloat("PosZ", vInput * speed / (2.0f * speed));

            animator.SetFloat("PosX", 0);

            //this is for the gravity
            //characterController.Move(mVelocity * Time.deltaTime);
        }
    }
}