using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementAbstractClass
{
    protected MovementStateManager stateManager; //keep track of this manager so that it change the state of it
    protected Transform transform; //this values are meant to be used in the states
    protected Animator animator;
    protected AmyMoveMentScript player;
    protected CharacterController characterController;

    //some times, the camera dictates what type of movement it has in the state. so keep a reference of it so
    //that it can be use by the state
    protected CameraType CameraType;

    //used for the start call
    private bool hasStarted = false;
    public MovementAbstractClass(CameraType cameraType, MovementStateManager stateManager)
    {
        CameraType = cameraType;
        player = AmyMoveMentScript.Instance;
        transform = player.transform;
        animator = player.Animator;
        characterController = player.CharacterController;
        this.stateManager = stateManager;
    }

    //when the state is first called
    protected virtual void Start()
    {

    }

    // The Update loop. Can be overrided to implement extra feature
    public virtual MovementAbstractClass Update()
    {
        if(!hasStarted)
        { //if the state has not run its start state, then run it first
            Start();
            hasStarted = true;
        }
        else
        { //else complete the update loop
            CompleteAction();
            //this is to apply any gravity to the character
            ApplyingVelocityToController();
        }
        //decide the next state of the controller
        var nextState = DecisionOfState();
        if(nextState != this)
        {//if it is different, then reset the has start bool so that the start method can be called again
            hasStarted = false;
            Exit();
        }
        return nextState;

    }

    //what are the thing to do in the update loop
    public abstract void CompleteAction();

    //extra things for FixUpdate loop
    public virtual void FixUpdate()
    {
        ApplyingGravity();
    }

    // called if the state is about to exit
    protected virtual void Exit(){}
    
    //used to decide what is the next state afterwards
    protected abstract MovementAbstractClass DecisionOfState();

    // changing the state of the camera state.

    #region methods
    //change the internal value of the camera for the state to use
    public void ChangeCamera(CameraType camera)
    {
        //change the movement based of camera
        CameraType = camera;

        ActionToTakeForDifferentCamera(camera);
    }

    private void ActionToTakeForDifferentCamera(CameraType camera)
    {
        //do some behaviour to different camera
        if (camera == CameraType.Follow_Track_Pos)
        {
            transform.rotation = Quaternion.identity;
        }
    }


    //apply physic related velocity to the character
    private void ApplyingVelocityToController()
    {
        //applying velocity to the character contoller at every Update
        var velocity = AmyMoveMentScript.Instance.Velocity;
        characterController.Move(velocity * Time.deltaTime);
    }
    //apply a downward force to the character in the fix update loop
    private void ApplyingGravity()
    {
        //creating a clone of the velocity so as to make changes to it and replace it
        Vector3 instanceVelocity = AmyMoveMentScript.Instance.Velocity;
        float gravity = AmyMoveMentScript.Instance.Gravity;

        //add downward velocity to the character.
        instanceVelocity.y += gravity * Time.deltaTime;

        //if it is already grounded, just add a bit of downward force to the character controller. 0.001f is just a constant;
        //because of some bug with the character controller it wont work if velocity is
        //set to 0 https://stackoverflow.com/questions/39732254/isgrounded-in-charactercontroller-not-stable
        if (characterController.isGrounded && instanceVelocity.y < 0) instanceVelocity.y = gravity * 0.001f;

        //setting up the velocity of the player.
        AmyMoveMentScript.Instance.Velocity = instanceVelocity;
    }

    //a shortcut to be used in the state 
    protected void RetrievingInputValues(
        out Vector2 inputValue, 
        out bool isAttacking,
        out bool isJumping,
        out bool isCrouching,
        out bool isReloading
        )
    {
        inputValue = AmyMoveMentScript.Instance.InputVector;
        isAttacking = AmyMoveMentScript.Instance.IsAttacking;
        isJumping = AmyMoveMentScript.Instance.CanJump;
        isCrouching = AmyMoveMentScript.Instance.CanCrouch;
        isReloading = AmyMoveMentScript.Instance.IsReloading;
    }

    #endregion
}
