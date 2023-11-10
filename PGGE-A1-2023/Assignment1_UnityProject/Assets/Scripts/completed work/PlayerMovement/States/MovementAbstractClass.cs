using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementAbstractClass
{
    protected CameraType CameraType;
    public MovementAbstractClass(CameraType cameraType)
    {
        CameraType = cameraType;
    }
    public abstract void CompleteAction();

    public void ChangeCamera(CameraType camera)
    {//change the movement based of camera
        camera = CameraType;
    }
}
