using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.completed_work.PlayerMovement.States
{
    public class IdleMovement : MovementAbstractClass
    {
        private List<AnimatorOverrideController> IdleAnimations;
        private string IdleAnimationName = "GroundBlendTree";
        private bool hasStartedInitialAnimation; //this is to see if it has started the starting idle animation
        private bool hasAnimationFinish;
        private bool isNotMoving; 
        public IdleMovement(CameraType cameraType, MovementStateManager stateManager) : base(cameraType, stateManager)
        {
            IdleAnimations = AmyMoveMentScript.Instance.IdleAnimations;
        }

        protected override void Start()
        {
            hasStartedInitialAnimation = false;
        }
        public override void CompleteAction()
        {
            SettingValues();
            IdleDecisionTree();
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

            if(isAttacking)
            {
                //if players left click to enable attacking
                return stateManager.attackingMovement;
            }
            else if(isJumping)
            {
                //if jumping is allowed
                 return stateManager.jumpingMovement;
            }
            else if(isCrouching)
            {
                //if players enabled crouching
                 return stateManager.crouchingMovement;
            }
            else if (isReloading)
            {
                //if player click on reloading
                 return stateManager.reloadMovement;
            }
            else if(input != Vector2.zero)
            {
                //if player have click on input
                 return stateManager.normalGroundMovement;
            }
            else
            {
                //if nothing is done, continue to do the idle animation
                return this;
            }

        }

        protected override void Exit()
        {
            hasStartedInitialAnimation = false;
        }

        #region idle movement methods
        // deciding what idle animation to complete
        private void IdleDecisionTree()
        {
            if(isNotMoving)
            {
                StartDoingIdleAnimation();
            }
            else
            {
                ReduceMovementAnimation();
            }
        }

        //setting the global values in every update loop
        private void SettingValues()
        {
            /*so check if the idle animation is finish. So it will consider the animation completed if 
            the animation reaches 95%
            */
            hasAnimationFinish = animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f &&
                animator.GetCurrentAnimatorStateInfo(0).IsTag(IdleAnimationName);

            //check if it has started moving
            isNotMoving = animator.GetFloat("PosX") == 0 &&
                animator.GetFloat("PosZ") == 0;
        }

        private void StartDoingIdleAnimation()
        {
            if (hasStartedInitialAnimation)
            {
                if (hasAnimationFinish) RandomiseIdleAnimation();
            }
            else
            {
                //so if not moving, has not started the initial animation
                //wait for all the moving animation to finish before starting its idle animation
                if (hasAnimationFinish) CompleteInitialIdleAnimationFirst();
            }
        }

        private void CompleteInitialIdleAnimationFirst()
        {
            //start with the first idle animation 
            animator.runtimeAnimatorController = IdleAnimations[0];
            animator.Play(IdleAnimationName, 0, 0);
            hasStartedInitialAnimation = true;
        }

        //this make sure that the animation looks like it slows down instead of an sudden stop
        private void ReduceMovementAnimation()
        {
            float posX = animator.GetFloat("PosX");
            float posZ = animator.GetFloat("PosZ");
            float dampingfactor = AmyMoveMentScript.Instance.InputDamper;
            if(posX > 0.2f)
            {
                posX -= dampingfactor * Time.deltaTime;
            }
            else if(posX < -0.2f)
            {
                posX += dampingfactor * Time.deltaTime;
            }
            else
            {
                posX = 0;
            }

            if(posZ > 0.2f)
            {
                posZ -= dampingfactor * Time.deltaTime;
            }
            else if(posZ < -0.2f)
            {
                posZ += dampingfactor * Time.deltaTime;
            }
            else
            {
                posZ = 0;
            }

            animator.SetFloat("PosX", posX);
            animator.SetFloat("PosZ", posZ);
        }

        //randomise the idle animation avaliable and start to play that animation
        private void RandomiseIdleAnimation()
        {
            int randomInt = (int) UnityEngine.Random.Range(0, IdleAnimations.Count );
            animator.runtimeAnimatorController = IdleAnimations[randomInt];
            animator.Play(IdleAnimationName, 0, 0);
        }

        #endregion
    }
}