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
        private float lastUpdateCall;
        public IdleMovement(CameraType cameraType) : base(cameraType)
        {
            IdleAnimations = AmyMoveMentScript.Instance.IdleAnimations;
        }

        public override void CompleteAction()
        {
            CheckingUpdatesAndResettingAnimation();

        }

        private void CheckingUpdatesAndResettingAnimation()
        {
            bool hasAnimationFinish = animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f &&
                animator.GetCurrentAnimatorStateInfo(0).IsTag(IdleAnimationName);

            bool isNotMoving = animator.GetFloat("PosX") == 0 &&
                animator.GetFloat("PosZ") == 0;

            if (Time.time - lastUpdateCall > 0.2f &&
                isNotMoving) //to make sure that the character is not moving
            {
                animator.runtimeAnimatorController = IdleAnimations[0]; //reset the animation back to usual idle
                animator.Play(IdleAnimationName, 0, 0);
            }
            else if (hasAnimationFinish && isNotMoving) 
            { //else that means that the animation has been playing for a long time
                RandomiseIdleAnimation();
            }s
            else
            { //else it means that the character just finish their movement so reduce the animation of the character.
                ReduceAnimation();
                lastUpdateCall = Time.time;
            }
        }

        private void ReduceAnimation()
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

        private void RandomiseIdleAnimation()
        {
            int randomInt = (int) UnityEngine.Random.Range(0, IdleAnimations.Count );
            animator.runtimeAnimatorController = IdleAnimations[randomInt];
            animator.Play(IdleAnimationName, 0, 0);
        }
    }
}