using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

namespace Assets.Scripts.completed_work.PlayerMovement.States
{
    public class AttackingMovement : MovementAbstractClass
    {
        private List<AttackSO> combo = new List<AttackSO>();
        private int comboCount; //combo count check how many times u click while in the attack animation
        private bool canStillAttack;
        public AttackingMovement(CameraType cameraType, MovementStateManager stateManager) : base(cameraType, stateManager)
        {
            combo = AmyMoveMentScript.Instance.Combo;
            //this is to set the combo reference
        }

        protected override void Start()
        {
            EndCombo(); //reset the combo
            Attack();
            //set it to false first, this is to see if the player will click the mouse again
        }

        public override void CompleteAction()
        {
            if (IsAttackAnimationPlaying(0.8f) )
            {
                HandleInput();
            }
            else if(canStillAttack)
            {
                Attack();
            }
        }
        protected override MovementAbstractClass DecisionOfState()
        {
            if (IsAttackAnimationPlaying())
            {
                //that mean the attack animation is still playing so continue to stay at this state
                return this;
            }
            else
            {
                return HandleNewState();
            }
        }

        private MovementAbstractClass HandleNewState()
        {
            RetrievingInputValues(
                out Vector2 input,
                out bool isAttacking,
                out bool isJumping,
                out bool isCrouching,
                out bool isReloading
                );

            if (isJumping)
            {
                //if jumping is allowed
                 return stateManager.jumpingMovement;
            }
            else if (isCrouching)
            {
                //if players enabled crouching
                 return stateManager.crouchingMovement;
            }
            else if (isReloading)
            {
                //if player click on reloading
                 return stateManager.reloadMovement;
            }
            else if (input != Vector2.zero)
            {
                //if player did nothing at all
                return stateManager.normalGroundMovement;
            }
            else
            {
                //if nothing is done, continue to do the idle animation
                 return stateManager.idleMovement;
            }
        }

        private void HandleInput()
        {
            bool clickAttackButton = AmyMoveMentScript.Instance.IsAttacking; 
            //attack represent if the player click the attack button

            if(IsAttackAnimationAboutToFinish(0.25f) && clickAttackButton)
            { //if the player has hit the attack button and the animation is 1/2 of its animation, then 
                //continue to play the next attack animation next.
                canStillAttack = true;
            }
            else
            {//ignore any attack click being made as it is too premature
                AmyMoveMentScript.Instance.AttackingDone();
            }
        }

        private void Attack()
        {  
            //play the attack animation. zero refering to the first attack to do before performing other combo
            animator.runtimeAnimatorController = combo[comboCount].controller;
            //Making the character to play the animation
            animator.Play("attack", 0, 0);

            comboCount++;
            //if there is no more combo to call out, reset to the first combo
            if (comboCount == combo.Count) comboCount = 0;

            //set it to false to make sure that the attack state dont continously run
            canStillAttack = false;


        }

        private bool IsAttackAnimationPlaying()
        {
            //check if the attack state progress
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.95f &&
            animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack");
        }

        //this function check the value if the animation has pass from 0 to a certain interval
        public bool IsAttackAnimationPlaying(float interval)
        {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime < interval &&
               animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack");
        }

        //this is the reverse 
        public bool IsAttackAnimationAboutToFinish(float interval)
        {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime > interval &&
              animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack");
        }

        private void EndCombo()
        {
            comboCount = 0;
        }

    }


        
    
}