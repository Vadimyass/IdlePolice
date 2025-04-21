using System;
using System.Collections.Generic;
using DG.Tweening;
using Gameplay.Scripts.Utils;
using TMPro;
using UnityEngine;

namespace Gameplay.Scripts.Buildings
{
    public class BuildingPlus : MonoBehaviour
    {
        [SerializeField] private Transform _plusTransform;
        [SerializeField] private List<Transform> _cornersTransform;
        [SerializeField] private Transform _shadowPlusTransform;
        [SerializeField] private TextMeshPro _nameText;
        [SerializeField] private TextMeshPro _nameShadowText;
        private Sequence _sequence;

        public void SetText(string text)
        {
            _nameText.text = text;
            _nameShadowText.text = text;
        }
        
        private void OnEnable()
        {
            _sequence = DOTween.Sequence();
            var plusSeq = DOTween.Sequence();
            plusSeq.Append(_plusTransform.DOMoveY(_plusTransform.position.y + 0.25f, 1).SetEase(Ease.Linear));
            plusSeq.Join(_plusTransform.DOScale(_plusTransform.localScale * 1.3f, 1).SetEase(Ease.Linear));
            plusSeq.Join(_shadowPlusTransform.DOScale(_shadowPlusTransform.localScale * 1.2f, 1).SetEase(Ease.Linear));
            plusSeq.Append(_plusTransform.DOMoveY(_plusTransform.position.y, 1).SetEase(Ease.Linear));
            plusSeq.Join(_plusTransform.DOScale(_plusTransform.localScale, 1).SetEase(Ease.Linear));
            plusSeq.Join(_shadowPlusTransform.DOScale(_shadowPlusTransform.localScale, 1).SetEase(Ease.Linear));
            plusSeq.SetLoops(-1);
            _sequence.Join(plusSeq);

            var pos2 = new Vector3(0, _plusTransform.localPosition.y, _plusTransform.localPosition.z);
            foreach (var cornerTransform in _cornersTransform)
            {
                var pos = new Vector3(0, cornerTransform.localPosition.y, cornerTransform.localPosition.z);
                
                var direction = (pos - pos2).normalized;
                var multiplier = (pos - pos2).y / direction.y;
                var seq = DOTween.Sequence();
                seq.Append(cornerTransform.DOLocalMove((direction * multiplier * 1.15f) + (Vector3.right * cornerTransform.localPosition.x), 1).SetEase(Ease.Linear));
                seq.Append(cornerTransform.DOLocalMove(cornerTransform.localPosition, 1).SetEase(Ease.Linear));
                seq.SetLoops(-1);

                _sequence.Join(seq);
            }
        }

        private void OnDisable()
        {
            _sequence.Kill();
        }
    }
}