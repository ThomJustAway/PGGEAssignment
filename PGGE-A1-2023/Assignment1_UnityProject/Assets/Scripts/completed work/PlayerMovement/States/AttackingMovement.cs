using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

namespace Assets.Scripts.completed_work.PlayerMovement.States
{
    public class AttackingMovement : MovementAbstractClass
    {
        private List<AttackSO> combo = new List<AttackSO>();
        private float lastClickTime;
        private int comboCount;
        public AttackingMovement(CameraType cameraType) : base(cameraType)
        {
            lastClickTime = Time.time;
            combo = AmyMoveMentScript.Instance.Combo; //good to have reference to this so that I dont have to keep on changing the value
        }

        public override void CompleteAction()
        {
            Attack();
        }

        private void Attack()
        {
            //add a function to reset the count
            if (Time.time - lastClickTime >= 0.5f && comboCount <= combo.Count )
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") || 
                    (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f &&
                     animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")
                    ))
                {
                    animator.runtimeAnimatorController = combo[comboCount].controller;
                    animator.Play("attack", 0, 0);

                    comboCount++;
                    lastClickTime = Time.time;
                    if (comboCount == combo.Count) comboCount = 0;
                    AttackFinish();
                }
            }
        }

        public bool IsAttackAnimationStillPlaying()
        {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9f &&
               animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack");
        }

        private void AttackFinish()
        {
            AmyMoveMentScript.Instance.AttackingDone();
        }


        private void EndCombo()
        {
            comboCount = 0;
        }
    }


        
    
}