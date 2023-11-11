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
        private float lastComboEnd;
        private int comboCount;
        private readonly string EndComboFunctionName = "EndCombo";
        private bool attack = false;
        public AttackingMovement(CameraType cameraType) : base(cameraType)
        {
            lastClickTime = Time.time;
            combo = AmyMoveMentScript.Instance.Combo; //good to have reference to this so that I dont have to keep on changing the value
        }

        public override void CompleteAction()
        {

        }

        private void Attack()
        {
            if (Time.time - lastClickTime >= 0.5f && comboCount <= combo.Count )
            {
                //CancelInvoke(EndComboFunctionName);
                //CancelInvoke("Move");
                if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
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

        private void AttackFinish()
        {
            AmyMoveMentScript.Instance.AttackingDone();
        }

        private void ExitAttack()
        {
            //if (mAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && mAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            //{
            //    Invoke(EndComboFunctionName, 1);
            //}
        }

        private void EndCombo() //called through invoke
        {
            comboCount = 0;
            lastComboEnd = Time.time;
        }
    }


        
    
}