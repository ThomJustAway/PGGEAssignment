﻿using Assets.Scripts.completed_work.PlayerMovement.States;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class MovementStateManager
{
    private CameraType cameraType;
    private ThirdPersonCamera cameraReference;
    private List<MovementAbstractClass> movements;

    #region movements

    private MovementAbstractClass currentState;
    public MovementClass normalGroundMovement { get; private set; }
    public JumpingMovement jumpingMovement {get; private set;}
    public CrouchingMovement crouchingMovement {get; private set;}
    public AttackingMovement attackingMovement {get; private set;}
    public ReloadMovement reloadMovement {get; private set;}
    public IdleMovement idleMovement {get; private set;}
    #endregion

    public MovementStateManager()
    {
        movements = new List<MovementAbstractClass>();
        cameraReference = Camera.main.GetComponent<ThirdPersonCamera>();
        cameraType = cameraReference.mCameraType;
        //canCrouch = AmyMoveMentScript.Instance.CanCrouch;

        //creating the movements
        normalGroundMovement = new MovementClass(cameraType , this);
        jumpingMovement = new JumpingMovement(cameraType , this);
        crouchingMovement = new CrouchingMovement(cameraType, this);
        attackingMovement = new AttackingMovement(cameraType , this);
        reloadMovement = new ReloadMovement(cameraType , this);
        idleMovement = new IdleMovement(cameraType, this);

        //adding them in a array so that I can make changes to all of them if the camera changes
        movements.Add(normalGroundMovement);
        movements.Add(jumpingMovement);
        movements.Add(crouchingMovement);
        movements.Add(attackingMovement);

        currentState = idleMovement;
    }

    public void HandleState()
    {
        if(currentState != null)
        {
            CheckCameraType();
            currentState = currentState.Update();
        }
    }

    public void HandleFixedUpdateState()
    {
        if(currentState != null)
        {
            currentState.FixUpdate();
        }
    }

    private void CheckCameraType()
    {
        if (cameraReference.mCameraType != cameraType)
        {
            Debug.Log("Camera change");
            cameraType = cameraReference.mCameraType;
            for (int i = 0; i < movements.Count; i++)
            {
                movements[i].ChangeCamera(cameraType);
            }
        }
    }

}
