using System;
using Audio;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Particles;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Gameplay.Scripts.Agents
{
    public class CarAgentBox : MonoBehaviour, IPointerDownHandler
    {
        private UnityAction _unityAction;
        private ParticleManager _particleManager;
        private bool _isOpen;
        private AudioManager _audioManager;

        public void Init(UnityAction unityAction, AudioManager audioManager, ParticleManager particleManager)
        {
            _audioManager = audioManager;
            _particleManager = particleManager;
            _unityAction = unityAction;
            _particleManager.PlayParticleInPosition(ParticleType.poof, transform.position, quaternion.identity, 10f);
        }
        
        private void Start()
        {
            var startPos = transform.position;
            var startRotation = transform.rotation.eulerAngles;
            var seq = DOTween.Sequence();
            seq.Append(transform.DOScaleY(0.6f, 0.2f));
            seq.Join(transform.DOScaleX(1.2f, 0.2f));
            seq.Append(transform.DOMoveY(startPos.y + 4, 0.2f).SetEase(Ease.OutCirc));
            seq.Join(transform.DOScaleY(1f, 0.2f));
            seq.Join(transform.DOScaleX(1f, 0.2f));
            seq.Append(transform.DOLocalRotate(startRotation + Vector3.forward * 15, 0.075f).SetEase(Ease.Linear));
            seq.Append(transform.DOLocalRotate(startRotation + Vector3.forward * -15, 0.15f).SetEase(Ease.Linear));
            seq.Append(transform.DOLocalRotate(startRotation + Vector3.forward * 15, 0.15f).SetEase(Ease.Linear));
            seq.Append(transform.DOLocalRotate(startRotation + Vector3.forward * -15, 0.15f).SetEase(Ease.Linear));
            seq.Append(transform.DOLocalRotate(startRotation, 0.075f).SetEase(Ease.Linear));
            seq.Append(transform.DOMoveY(startPos.y, 0.1f).SetEase(Ease.OutCirc));
            seq.Append(transform.DOScaleY(0.6f, 0.2f));
            seq.Join(transform.DOScaleX(1.2f, 0.2f));
            seq.Append(transform.DOScaleY(1f, 0.2f));
            seq.Join(transform.DOScaleX(1f, 0.2f));
            seq.AppendInterval(2);

            seq.SetLoops(-1);
        }


        public async void OnPointerDown(PointerEventData eventData)
        {
            if(_isOpen == true) return;

            _audioManager.PlaySound(TrackName.OpenCarBox);
            _isOpen = false;
            gameObject.SetActive(false);
            _particleManager.PlayParticleInPosition(ParticleType.poof, transform.position, quaternion.identity, 10f);
            _unityAction();
        }
    }
}