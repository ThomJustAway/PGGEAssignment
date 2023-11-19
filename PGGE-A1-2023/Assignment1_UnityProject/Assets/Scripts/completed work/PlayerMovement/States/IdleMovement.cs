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
            lastUpdateCall = Time.time;
        }

        public override void CompleteAction()
        {
            CheckingUpdatesAndResettingAnimation();
            lastUpdateCall = Time.time;
            bool hasAnimationFinish = animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f &&
                animator.GetCurrentAnimatorStateInfo(0).IsTag(IdleAnimationName);

            ResetAnimation();
            if (hasAnimationFinish)
            {
                RandomiseIdleAnimation();
            }

        }

        private void CheckingUpdatesAndResettingAnimation()
        {
            if (Time.time - lastUpdateCall > 0.1f)
            {
                ResetAnimation();
                animator.runtimeAnimatorController = IdleAnimations[0]; //reset the animation back to usual idle
                animator.Play(IdleAnimationName, 0, 0);
            }
        }

        private void ResetAnimation()
        {
            animator.SetFloat("PosX", 0);
            animator.SetFloat("PosZ", 0);
        }

        private void RandomiseIdleAnimation()
        {
            int randomInt = (int) UnityEngine.Random.Range(0, IdleAnimations.Count - 1);
            animator.runtimeAnimatorController = IdleAnimations[randomInt];
            animator.Play(IdleAnimationName, 0, 0);

        }
    }
}