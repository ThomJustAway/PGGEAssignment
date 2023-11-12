using System.Collections;
using UnityEngine;

namespace Assets.Scripts.completed_work.PlayerMovement.States
{
    public class ReloadMovement : MovementAbstractClass
    {
        private string key = "Reload";
        public ReloadMovement(CameraType cameraType) : base(cameraType)
        {
        }

        public override void CompleteAction()
        {
            animator.SetTrigger(key);
            AmyMoveMentScript.Instance.FinishReloading();
        }
        
        public bool IsAnimationStillPlaying()
        {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9f &&
               animator.GetCurrentAnimatorStateInfo(0).IsTag(key);
        }
    }
}