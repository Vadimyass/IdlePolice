using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Scripts.Utils;
using UnityEngine;

namespace Gameplay.Scripts.Buildings
{
    public class BuildingAnimation : MonoBehaviour
    {
        [SerializeField] private Transform _buidingTransform;
        private Sequence _seq;


        public void BuildAnimation()
        {
            var scale = _buidingTransform.localScale;
            _seq = DOTween.Sequence();
            _seq.Append(_buidingTransform.DOScaleY(scale.y * 1.5f, 1.5f).From(Vector3.zero).SetEase(Ease.InBounce));
            _seq.Join(_buidingTransform.DOScaleZ(scale.z * 1f, 1f).From(Vector3.zero).SetEase(Ease.InBounce));
            _seq.Join(_buidingTransform.DOScaleX(scale.x * 1f, 1f).From(Vector3.zero).SetEase(Ease.InBounce));
            _seq.Append(_buidingTransform.DOScaleY(scale.y, 0.5f).SetEase(Ease.OutBounce));
        }

        public void StartAnimation()
        {
            if (_seq == null)
            {
                _seq = DOTween.Sequence();
                _seq.Append(_buidingTransform.DOScaleY(_buidingTransform.localScale.y * 1.5f, 0.5f).SetEase(Ease.Linear));
                _seq.Append(_buidingTransform.DOScaleY(_buidingTransform.localScale.y, 0.5f).SetEase(Ease.Linear));
                _seq.SetLoops(-1);
            }

            if(_seq.IsPlaying() == true) return;
            
            _seq.Play();
        }

        public async void PauseAnimation()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            _seq.Pause();
        }
        
    }
}