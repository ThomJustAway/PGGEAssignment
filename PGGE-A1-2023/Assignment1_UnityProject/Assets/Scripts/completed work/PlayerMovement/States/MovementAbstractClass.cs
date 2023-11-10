using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementAbstractClass
{
    protected Transform transform;
    protected Animator animator;
    protected AmyMoveMentScript player;
    protected CharacterController characterController;
    protected CameraType CameraType;
    public MovementAbstractClass(CameraType cameraType)
    {
        CameraType = cameraType;
        player = AmyMoveMentScript.Instance;
        transform = player.transform;
        animator = player.Animator;
        characterController = player.CharacterController;
    }
    public abstract void CompleteAction();

    public void ChangeCamera(CameraType camera)
    {//change the movement based of camera
        CameraType = camera;
    }
}
