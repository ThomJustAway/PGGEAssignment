using System;
using System.Collections;
using System.Collections.Generic;
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

            Vector3 newPlayerVector = PlayerTransform.position; //as vector3 are value type, I dont have to make a new vector 3;
            newPlayerVector.y += CameraConstants.playerHeight; 

            Vector3 direction = newPlayerVector - CameraTransform.transform.position; 
            Ray pointer = new Ray(CameraTransform.transform.position, direction);

            //Debug.DrawRay(CameraTransform.transform.position, direction, Color.blue); //uncomment it for debug. for visual purpose

            float padding = 0.5f; //padding to make sure the raycast ignore the player's collider

            if (Physics.Raycast(pointer , out RaycastHit hitobject , direction.magnitude - padding ))
            {
                //if there is a hit
                Debug.Log($"hit {hitobject.transform.name} Distance: { hitobject.distance}");
                Vector3 offset = direction.normalized * hitobject.distance; 
                //going to find the distance require to resolve the hit and offset it for the camera
                CameraTransform.transform.position = CameraTransform.transform.position + offset;
            }

        }

        public abstract void Update();
    }
}
