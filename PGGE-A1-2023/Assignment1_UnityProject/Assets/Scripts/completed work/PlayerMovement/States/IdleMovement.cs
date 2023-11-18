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
        public IdleMovement(CameraType cameraType) : base(cameraType)
        {
            IdleAnimations = AmyMoveMentScript.Instance.IdleAnimations;
        }

        public override void CompleteAction()
        {
            bool hasAnimationFinish = animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f &&
                animator.GetCurrentAnimatorStateInfo(0).IsTag(IdleAnimationName);

            ResetAnimation();
            if(hasAnimationFinish)
            {
                RandomiseIdleAnimation();
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