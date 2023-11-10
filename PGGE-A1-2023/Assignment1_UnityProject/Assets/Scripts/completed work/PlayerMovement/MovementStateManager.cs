using Assets.Scripts.completed_work.PlayerMovement.States;
using System.Collections;
using System.Collections.Generic;
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
    #endregion

    private bool canJump;

    public MovementStateManager() 
    { 
        movements = new List<MovementAbstractClass>();
        cameraReference = Camera.main.GetComponent<ThirdPersonCamera>();
        cameraType = cameraReference.mCameraType;
        normalGroundMovement = new MovementClass(cameraType);
        jumpingMovement = new JumpingMovement(cameraType);
        physicMovement = new PhysicMovement(cameraType);
        movements.Add(normalGroundMovement);
        movements.Add(jumpingMovement);
    }

    public void Move()
    {
        canJump = AmyMoveMentScript.Instance.canJump;
        if(canJump)
        {
            jumpingMovement.CompleteAction();
            normalGroundMovement.CompleteAction();
        }
        else
        {
            normalGroundMovement.CompleteAction();
        }

        //Add physic to the amy
        physicMovement.CompleteAction();
    }

    public void CheckCameraType()
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
}
