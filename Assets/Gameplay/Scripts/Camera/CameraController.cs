using System;
using System.IO;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Scripts.Utils;
using Lean.Touch;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Gameplay.Scripts
{
    public class CameraController
    {
        private Camera _currentCamera;
        
        private Transform _parent;
        private CameraConfig _cameraConfig;
        private InputController _inputController;
        private LeanCameraZoom _leanCameraZoom;
        private LeanDragCamera _leanDragCamera;
        private bool isUnfollowed = false;
        private int _lastFov;
        [Inject] private DiContainer _container;
        //private readonly AudioMixer _audioMixerGroup;

        
        
        public CameraController(CameraConfig cameraConfig)
        {
            _cameraConfig = cameraConfig;
            //_audioMixerGroup = audioMixerGroup;
        }
        
        public void EnabledLeanDragCamera(bool isEnable)
        {
            _leanDragCamera.enabled = isEnable;
        }

        public void EnabledLeanCameraZoom(bool isEnable)
        {
            _leanCameraZoom.enabled = isEnable;
        }
        
        public void SetBordersForDragCamera(Vector3 maxBorder, Vector3 minBorder, Vector3 maxBorder2, Vector3 minBorder2)
        {
            _leanDragCamera.SetBorders(maxBorder, minBorder, maxBorder2, minBorder2);
        }

        public void ShakeCamera(float duration = 0.5f)
        {
            _currentCamera.transform.DOShakeRotation(duration, 5);
        }

        public void MoveCameraToPosition(Vector3 position)
        {
            _currentCamera.transform.DOMove(position, 1);
        }
        
        public void ChangeFieldOfView(int fieldOfView)
        {
            _currentCamera.DOFieldOfView(fieldOfView, 0.5f).SetEase(Ease.Linear);
        }

        public void ComeBackFieldOfView()
        {
            ChangeFieldOfView(_lastFov);
        }
        
        public async UniTask FocusOnPosition(Vector3 position, bool rememberFov = false)
        {
            if (rememberFov == true)
            {
                _lastFov = (int)_currentCamera.fieldOfView;
            }
            var newCameraPos = new Vector3(position.x-33, _currentCamera.transform.position.y, position.z-55);
            _currentCamera.transform.DOMove(newCameraPos, 0.5f).SetUpdate(true);
            ChangeFieldOfView(25);
        }
        
        public void FocusOnTransform(Transform transform, bool rememberFov = false)
        {
            if(transform == null) return;
            
            if (rememberFov == true)
            {
                _lastFov = (int)_currentCamera.fieldOfView;
            }
            var newCameraPos = new Vector3(transform.position.x-33, _currentCamera.transform.position.y, transform.position.z -55);
            _currentCamera.transform.DOMove(newCameraPos, 0.5f).SetUpdate(true);
            ChangeFieldOfView(25);
        }
        
        public void FocusOnBuilding(Transform transform, bool rememberFov = false, int fov = 20)
        {
            if (rememberFov == true)
            {
                _lastFov = (int)_currentCamera.fieldOfView;
            }
            var newCameraPos = new Vector3(transform.position.x-33, _currentCamera.transform.position.y, transform.position.z-55);
            _currentCamera.transform.DOMove(newCameraPos, 0.5f).SetUpdate(true);;
            ChangeFieldOfView(fov);
        }

        public void SetTransformCamera(Vector3 position)
        {
            _currentCamera.transform.position = new Vector3(position.x, _currentCamera.transform.position.y, position.z);
        }

        public async UniTask FocusOnObject(Transform target)
        {
            await UniTask.WaitForFixedUpdate();
            isUnfollowed = false;
            Vector3 newCameraPos;
            do
            {
                newCameraPos = new Vector3(target.position.x - 1.75f, _currentCamera.transform.position.y,
                    target.position.z + 2.25f);
                _currentCamera.transform.position = Vector3.MoveTowards(_currentCamera.transform.position, newCameraPos, 2f);
                await UniTask.WaitForFixedUpdate();
            } 
            while (isUnfollowed == false);
        }
        
        public void StopCameraFollowing()
        {
            isUnfollowed = true;
        }

        public void SetDefaultParent()
        {
            Camera.main.transform.SetParent(_parent);
        }
        

        public void CreateNormalCamera()
        {
            if (_currentCamera != null)
            {
                GameObject.Destroy(_currentCamera.gameObject);
            }

            _currentCamera = _container.InstantiatePrefab(_cameraConfig.GetNormalCameraPrefab()).GetComponent<Camera>();
            _parent = _currentCamera.transform.parent;
    
            _leanDragCamera = _currentCamera.GetComponent<LeanDragCamera>();
            _leanCameraZoom = _currentCamera.GetComponent<LeanCameraZoom>();

            //_leanCameraZoom.AudioMixerGroup = _audioMixerGroup;
        }
    }
}