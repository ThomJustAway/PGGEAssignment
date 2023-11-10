using Assets.Scripts.completed_work.PlayerMovement.States;
using System.Collections;
using UnityEngine;


public class MovementStateManager
{
    private MovementClass walkingMovement;
    public MovementStateManager() 
    { 
        CameraType cameraType = Camera.main.GetComponent<ThirdPersonCamera>().mCameraType;
        walkingMovement = new MovementClass(cameraType);
    }

    public void Move()
    {
        walkingMovement.CompleteAction();
    }

}
