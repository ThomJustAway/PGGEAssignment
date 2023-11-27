using System.Collections;
using UnityEngine;
using UnityEngine.Windows;

public class JumpingMovement : MovementAbstractClass
{
    public JumpingMovement(CameraType cameraType) : base(cameraType){
        
    }

    public override void CompleteAction()
    {
        var jumpheight = AmyMoveMentScript.Instance.JumpHeight;
        var gravity = AmyMoveMentScript.Instance.Gravity;
        animator.SetTrigger("Jump");
        Vector3 newGravitationForce = AmyMoveMentScript.Instance.Velocity;
        newGravitationForce.y += Mathf.Sqrt(jumpheight * -2f * gravity);
        AmyMoveMentScript.Instance.Velocity += newGravitationForce;
        //stopping the jump
        AmyMoveMentScript.Instance.StopJump();
    }


}
