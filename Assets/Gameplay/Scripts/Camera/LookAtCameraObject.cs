using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Gameplay.Scripts
{
    public class LookAtCameraObject : MonoBehaviour
    {
        private TweenerCore<Quaternion, Vector3, QuaternionOptions> _sequence;
        private Quaternion _rotation;

        private void Awake()
        {
            _rotation = transform.rotation;
        }

        private void LateUpdate()
        {
            if (gameObject.activeSelf)
            {
                transform.rotation =
                    Quaternion.Euler(Camera.main.transform.rotation.eulerAngles - Vector3.right * 34 - Vector3.up * 185 - Vector3.forward * 7);
            }
        }
    }
}