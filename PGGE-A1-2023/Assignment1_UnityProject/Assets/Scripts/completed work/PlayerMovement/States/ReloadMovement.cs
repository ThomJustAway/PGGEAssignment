using System.Collections;
using UnityEngine;

namespace Assets.Scripts.completed_work.PlayerMovement.States
{
    public class ReloadMovement : MovementAbstractClass
    {
        private string key = "Reload";

        public ReloadMovement(CameraType cameraType, MovementStateManager stateManager) : base(cameraType, stateManager)
        {
        }

        protected override void Start()
        {
            animator.SetTrigger(key);
            AmyMoveMentScript.Instance.FinishReloading();
        }

        public override void CompleteAction()
        {
            PreventStackingReload();
        }

        protected override MovementAbstractClass DecisionOfState()
        {
            if(IsAnimationStillPlaying())
            {
                //so if the reload animation is still playing, then continue to be in the reload state
                return this;
            }
            else
            {
                return SelectNewState();
            }
        }

        private MovementAbstractClass SelectNewState()
        {
            //process the next set of input to see which state should be the next state
            RetrievingInputValues(
                            out Vector2 input,
                            out bool isAttacking,
                            out bool isJumping,
                            out bool isCrouching,
                            out bool isReloading
                            );

            if (isAttacking)
            {
                //if players left click to enable attacking
                 return stateManager.attackingMovement;
            }
            else if (isJumping)
            {
                //if jumping is allowed
                 return stateManager.jumpingMovement;
            }
            else if (isCrouching)
            {
                //if players enabled crouching
                 return stateManager.crouchingMovement;
            }
            else if (input != Vector2.zero)
            {
                //if player is moving
                return stateManager.normalGroundMovement;
            }
            else
            {
                //this would mean that the player is not doing anything, trigger idle animation
                return stateManager.idleMovement;
            }
        }

        //this function makes sure that u can only call the reload once and that is
        //while it is not in the reload movement
        private void PreventStackingReload()
        {
            if(AmyMoveMentScript.Instance.IsReloading)
            {
                AmyMoveMentScript.Instance.FinishReloading();
            }
        }

        //this function is used to check if the character is still in the reload animation
        public bool IsAnimationStillPlaying()
        {
            //see if the reload animation is complete, return true if it is still playing else return false
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f &&
               animator.GetCurrentAnimatorStateInfo(0).IsTag(key);
        }
    }
}