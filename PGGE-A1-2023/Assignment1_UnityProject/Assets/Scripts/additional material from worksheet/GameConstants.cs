using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PGGE
{
    public static class CameraConstants
    {
        public static Vector3 CameraAngleOffset { get; set; }
        public static Vector3 CameraPositionOffset { get; set; }
        public static float Damping { get; set; }
        public static float RotationSpeed { get; set; }
        public static float MinPitch { get; set; }
        public static float MaxPitch { get; set; }

        public static Transform CameraReferencePoint { get; set; } //CameraReferencePoint is for raycasting to the model to make it accurate


    }
    public static class PlayerConstants
    {
        public static LayerMask PlayerMask { get; set; }
    }
}
