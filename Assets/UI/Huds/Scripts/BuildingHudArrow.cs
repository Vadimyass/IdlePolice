using System;
using Agents;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Particles;
using UnityEngine;
using Zenject;

namespace UI.Huds.Scripts
{
    public class BuildingHudArrow : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Material _materialDefault;
        [SerializeField] private Material _activateMaterial;
        [SerializeField] private Transform _outTransform;
        [SerializeField] private Transform _prisonerCard;
        [SerializeField] private Transform _transformForCard;
        private Sequence _sequence;
        private Vector3 _startPos;


        private void Awake()
        {
            _prisonerCard.gameObject.SetActive(false);
            _startPos = transform.localPosition;
        }

        public async UniTask StartInAnimation(CarAgent carAgent)
        {
            _sequence.Kill();
            
            _prisonerCard.position = carAgent.transform.position;
            _prisonerCard.gameObject.SetActive(true);
            _prisonerCard.rotation = Quaternion.Euler(carAgent.transform.rotation.eulerAngles + Vector3.right * 90);
            
            _meshRenderer.sharedMaterial = _activateMaterial;
            
            _sequence = DOTween.Sequence();
            
            _sequence.Append(transform.DOLocalMoveX(_outTransform.transform.localPosition.x, 0.25f).From(_startPos.x));

            _sequence.AppendCallback(() => carAgent.CriminalHud?.HideImage());
            
            var seq = DOTween.Sequence();
            seq.Append(carAgent.transform.DOScaleY(carAgent.transform.localScale.y * 1.2f, 0.2f));
            seq.Append(carAgent.transform.DOScaleY(carAgent.transform.localScale.y, 0.2f));

            _sequence.Append(seq);
            
            _sequence.AppendCallback(() => _prisonerCard.parent = _transformForCard);
            
            _sequence.Append(_prisonerCard.DORotate(_transformForCard.rotation.eulerAngles, 0.4f));
            _sequence.Join(_prisonerCard.DOLocalMove(Vector3.zero, 0.4f));
            
            _sequence.Append(transform.DOLocalMoveX(_startPos.x, 0.25f).From(_outTransform.transform.localPosition.x).SetEase(Ease.OutElastic));
            
            _sequence.AppendCallback((() =>
            {
                _meshRenderer.sharedMaterial = _materialDefault;
                _prisonerCard.parent = _transformForCard.parent;
                _prisonerCard.gameObject.SetActive(false);
            }));

            await _sequence.AsyncWaitForCompletion();
        }
        
        public async UniTask StartOutAnimation(Transform endParticlePosition)
        {
            _meshRenderer.sharedMaterial = _activateMaterial;
            _prisonerCard.position = _transformForCard.position;
            _prisonerCard.parent = _transformForCard;
            _prisonerCard.rotation = _transformForCard.rotation;
            _prisonerCard.gameObject.SetActive(true);
            
            await transform.DOLocalMoveX(_outTransform.transform.localPosition.x, 0.25f).From(0).AsyncWaitForCompletion();

            _prisonerCard.parent = _transformForCard.parent;
            _prisonerCard.DOMoveZ(endParticlePosition.position.z, 0.4f);
            _prisonerCard.DORotate(endParticlePosition.rotation.eulerAngles + Vector3.right*90, 0.4f);
            await _prisonerCard.DOMoveX(endParticlePosition.position.x, 0.4f).AsyncWaitForCompletion();
            
            await _prisonerCard.DOMoveY(endParticlePosition.position.y, 0.2f).SetEase(Ease.OutCirc).AsyncWaitForCompletion();
            
            var seq = DOTween.Sequence();
            seq.Append(endParticlePosition.DOScaleY(endParticlePosition.localScale.y * 1.2f, 0.2f));
            seq.Append(endParticlePosition.DOScaleY(endParticlePosition.localScale.y, 0.2f));
            _prisonerCard.gameObject.SetActive(false);
            _meshRenderer.sharedMaterial = _materialDefault;
            await transform.DOLocalMoveX(0, 0.25f).From(_outTransform.transform.localPosition.x).SetEase(Ease.OutElastic).AsyncWaitForCompletion();
        }
    }
}