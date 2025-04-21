using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Scripts
{
    public class CameraConfig : ScriptableObject
    {
        [SerializeField] private Camera _normalCameraPrefab;
        
        public Camera GetNormalCameraPrefab()
        {
            return _normalCameraPrefab;
        }
    }

    public enum CameraPath
    {
        BusFollow,
        InstructorAndSoldierFollow,
        DeserterFollow,
        DeserterFollow2,
    }
}