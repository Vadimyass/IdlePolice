using UnityEngine;

namespace Gameplay.Scripts
{
    public class CameraPathFactory
    {
        private Transform _parentTransform;
        private CameraConfig _cameraConfig;

        public CameraPathFactory(CameraConfig cameraConfig)
        {
            _cameraConfig = cameraConfig;
        }
        
        public void Init(Transform transform)
        {
            _parentTransform = transform;
        }
        
    }
}