using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Windows;

public class JumpingMovement : MovementAbstractClass
{
    private bool staticJumping;
    private bool hasAddForce;
    private string keyForPosX = "PosX";
    private string keyForPosZ = "PosZ";
    private string tag = "jump";
    private float jumpheight;
    private float gravity;
    public JumpingMovement(CameraType cameraType, MovementStateManager stateManager) : base(cameraType, stateManager)
    {
        jumpheight = AmyMoveMentScript.Instance.JumpHeight;
        gravity = AmyMoveMentScript.Instance.Gravity;
    }

    protected override void Start()
    {
        staticJumping = false;
        animator.SetTrigger("Jump");

        //check if the jump is at a stand still jump or is there a velocity applied to the jump before hand
        CheckForStaticJumping();
        
        if(!staticJumping)
        {
            //if it not a static jump, then just add the force to the jump without any delay
            Vector3 jumpForce = AmyMoveMentScript.Instance.Velocity;
            jumpForce.y += Mathf.Sqrt(jumpheight * -2f * gravity);
            AmyMoveMentScript.Instance.Velocity += jumpForce;
        }
        else
        {
            hasAddForce = false;
        }
        //stopping the jump bool
        AmyMoveMentScript.Instance.StopJump();
    }


    private void CheckForStaticJumping()
    {
        float valueFromAnimatorZ = animator.GetFloat(keyForPosZ);
        float valueFromAnimatorX = animator.GetFloat(keyForPosX);

        if (valueFromAnimatorZ == 0 && valueFromAnimatorX == 0) staticJumping = true;
        else staticJumping = false;
        //Debug.Log($"static jumping value: {staticJumping}");
    }

    public override void CompleteAction()
    {
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        if (IsAnimationStillPlaying(1f))
        {
            //when the animation reach at least 40 % of the way, then add the force
            Debug.Log("code ran");
            if (AnimationReachACertainPoint(0.4f) && !hasAddForce)
            {
                //add force the the player
                Vector3 jumpForce = AmyMoveMentScript.Instance.Velocity;
                jumpForce.y += Mathf.Sqrt(jumpheight * -2f * gravity);
                AmyMoveMentScript.Instance.Velocity += jumpForce;
                //only add this force only once
                hasAddForce = true;
            }
        }
        else
        {
            staticJumping = false;
        }
    }

    protected override MovementAbstractClass DecisionOfState()
    {
        if(staticJumping) { return this; }

        RetrievingInputValues(
                out Vector2 input,
                out bool isAttacking,
                out bool isJumping,
                out bool isCrouching,
                out bool isReloading
                );

        if (input != Vector2.zero)
        {
            //if player is trying to move
            return stateManager.normalGroundMovement;
        }
        else
        {
            //if nothing is done, continue to do the idle animation
             return stateManager.idleMovement;
        }
    }

    public bool IsAnimationStillPlaying(float interval)
    {
        //see if the static jump animation is complete, return true if it is still playing else return false
        bool value = animator.GetCurrentAnimatorStateInfo(0).normalizedTime < interval;
        return value && animator.GetCurrentAnimatorStateInfo(0).IsTag(tag);
    }

    public bool AnimationReachACertainPoint(float point)
    {
        return 
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= point &&
           animator.GetCurrentAnimatorStateInfo(0).IsTag(tag);
    }
}
