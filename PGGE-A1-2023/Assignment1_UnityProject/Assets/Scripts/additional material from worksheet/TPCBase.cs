using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace PGGE
{
    // The base class for all third-person camera controllers
    public abstract class TPCBase
    {
        protected Transform mCameraTransform;
        protected Transform mPlayerTransform;

        public Transform CameraTransform
        {
            get
            {
                return mCameraTransform;
            }
        }
        public Transform PlayerTransform
        {
            get
            {
                return mPlayerTransform;
            }
        }

        public TPCBase(Transform cameraTransform, Transform playerTransform)
        {
            mCameraTransform = cameraTransform;
            mPlayerTransform = playerTransform;
        }

        public void RepositionCamera()
        {
            //-------------------------------------------------------------------
            // Implement here.
            //-------------------------------------------------------------------
            //-------------------------------------------------------------------
            // Hints:
            //-------------------------------------------------------------------
            // check collision between camera and the player.
            // find the nearest collision point to the player
            // shift the camera position to the nearest intersected point
            //-------------------------------------------------------------------

            //this value is to prevent the raycast to not point at the play's feet

            //Vector3 newPlayerVector = CameraConstants.CameraReferencePoint.position;


            //Vector3 direction = newPlayerVector - CameraTransform.transform.position; 
            //Ray pointer = new Ray(CameraTransform.transform.position, direction);

            Vector3 newPlayerVector = CameraConstants.CameraReferencePoint.position; 
            //this reference point is at the top of the head
            //so the ray cast will raycast from camera to the head of the model

            float padding = 0.3f; //padding to make sure the raycast ignore the player's collider

            //finding the direction from the player to the camera position
            Vector3 direction =  CameraTransform.transform.position - newPlayerVector; 
            newPlayerVector += direction * 0.1f; //add some padding so that it does not accidentally hit the player collider

            Ray pointer = new Ray(newPlayerVector, direction);

            //Debug.DrawRay(newPlayerVector, direction, Color.blue); //uncomment it for debug. for visual purpose

            if (Physics.Raycast(pointer , out RaycastHit hitobject , direction.magnitude , LayerMask.GetMask( "Default" ))) //will ignore glass materia
            {
                //Vector3 offset = direction.normalized * hitobject.distance;

                //newDistance -= Offset;
                //the hit object point is the clostest point from the player so make the camera move there
                CameraTransform.position = hitobject.point;
            }
        }

        public abstract void Update();
    }
}
