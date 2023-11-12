using Assets.Scripts.completed_work.PlayerMovement.States;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;


public class MovementStateManager
{
    private CameraType cameraType;
    private ThirdPersonCamera cameraReference;
    private List<MovementAbstractClass> movements;

    #region movements
    private MovementClass normalGroundMovement;
    private JumpingMovement jumpingMovement;
    private PhysicMovement physicMovement;
    private CrouchingMovement crouchingMovement;
    private AttackingMovement attackingMovement;
    private ReloadMovement reloadMovement;
    #endregion

    private bool canJump;
    private bool canCrouch;
    private bool canAttack;
    private bool attackAnimationStillPlaying;
    private bool canReload;
    private bool reloadAnimationStillPlaying;

    public MovementStateManager() 
    { 
        movements = new List<MovementAbstractClass>();
        cameraReference = Camera.main.GetComponent<ThirdPersonCamera>();
        cameraType = cameraReference.mCameraType;
        canCrouch = AmyMoveMentScript.Instance.CanCrouch;

        //creating the movements
        normalGroundMovement = new MovementClass(cameraType);
        jumpingMovement = new JumpingMovement(cameraType);
        physicMovement = new PhysicMovement(cameraType);
        crouchingMovement = new CrouchingMovement(cameraType);
        attackingMovement = new AttackingMovement(cameraType);
        reloadMovement = new ReloadMovement(cameraType);

        //adding them in a array so that I can make changes to all of them if the camera changes
        movements.Add(normalGroundMovement);
        movements.Add(jumpingMovement);
        movements.Add(crouchingMovement);
        movements.Add(attackingMovement);
    }

    public void Move()
    {
        CheckCameraType(); //make sure it has the correct cameratype
        CheckCrouch(); //check if can crouch

        canJump = AmyMoveMentScript.Instance.CanJump; //cant have player crouching and jumping at the same time.
        canAttack = AmyMoveMentScript.Instance.IsAttacking;
        canReload = AmyMoveMentScript.Instance.IsReloading;

        attackAnimationStillPlaying = attackingMovement.IsAttackAnimationStillPlaying();
        reloadAnimationStillPlaying = reloadMovement.IsAnimationStillPlaying();

        if (canCrouch )
        {//only move because the crouch has been settled by the checkCrouch movement.
            crouchingMovement.CompleteAction();
        }//do crouching
        else if (canAttack || attackAnimationStillPlaying )
        {
            attackingMovement.CompleteAction();
        }
        else if(canJump )
        {
            jumpingMovement.CompleteAction();
            normalGroundMovement.CompleteAction();
        } //do jumping 
        else if (canReload )
        {
            reloadMovement.CompleteAction();
        }
        else if(!reloadAnimationStillPlaying)
        {
            normalGroundMovement.CompleteAction();
        } //do normal walking

        //Add physic to the amy
        physicMovement.CompleteAction();
    }

    private void CheckCameraType()
    {
        if(cameraReference.mCameraType != cameraType)
        {
            Debug.Log("Camera change");
            cameraType = cameraReference.mCameraType;
            for(int i = 0; i < movements.Count; i++)
            {
                movements[i].ChangeCamera(cameraType);
            }
        }
    }

    private void CheckCrouch()
    {
        if(canCrouch != AmyMoveMentScript.Instance.CanCrouch)
        {
            canCrouch = AmyMoveMentScript.Instance.CanCrouch;
            crouchingMovement.ChangeCrouchingValue(canCrouch);
        }
    }
}
