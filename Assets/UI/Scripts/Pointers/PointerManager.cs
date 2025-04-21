using System;
using DG.Tweening;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Object = UnityEngine.Object;

namespace Pointers
{
    public class PointerManager : MonoBehaviour, IDisposable
    {
        [SerializeField] private Image _pointerPrefab;
        [SerializeField] private int _maximumPointersOnScreen;
        private int _currentPointersCount;
        private bool _isSeqStopped;
        private UIManager _uiManager;

        [Inject]
        private void Construct(UIManager uiManager)
        {
            _uiManager = uiManager;
        }
        
        public Sequence AnimateHand(RectTransform button, Side sideToMove)
        {
            if(_currentPointersCount >= _maximumPointersOnScreen) return null;
            var seq = DOTween.Sequence();
            var pointer = Instantiate(_pointerPrefab, button);
            _currentPointersCount++;
            var startPosition = Vector3.zero;
            var endPosition = Vector3.zero;
            switch (sideToMove)
            {
                case (Side.Left):
                {
                    endPosition = new Vector3(0 - button.rect.width/2, 0);
                    startPosition = new Vector3(endPosition.x - button.rect.width / 2, endPosition.y);
                    pointer.transform.Rotate(Vector3.back*90);
                    break;  
                }
                case (Side.Right):
                {
                    endPosition = new Vector3(button.rect.width/2, 0);
                    startPosition = new Vector3(endPosition.x + button.rect.width / 2, endPosition.y);
                    pointer.transform.Rotate(Vector3.forward*90);
                    break;
                }
                case (Side.Down):
                {
                    endPosition = new Vector3(0, 0 - button.rect.height/2);
                    startPosition = new Vector3(endPosition.x, endPosition.y - button.rect.height / 2);
                    break;
                }
                case (Side.Top):
                {
                    pointer.transform.rotation = Quaternion.Euler(Vector3.forward * 180);
                    endPosition = new Vector3(0, 0 + button.rect.height/2);
                    startPosition = new Vector3(endPosition.x, endPosition.y + button.rect.height / 2);
                    break;
                }
            }
            pointer.transform.localPosition = startPosition;
            seq.Append(pointer.transform.DOLocalMove(endPosition, 0.75f));
            seq.Append(pointer.transform.DOLocalMove(startPosition, 0.75f));
            seq.AppendCallback(() =>
            {
                Destroy(pointer.gameObject);
                _currentPointersCount--;
            });
            seq.SetUpdate(true);
            seq.onKill += () =>
            {
                Destroy(pointer.gameObject);
                _currentPointersCount--;
            };
            return seq;
        }

        public Sequence MoveFromOnePointToOtherPoint(Vector3 startPos, Vector3 endPos)
        {
            var seq = DOTween.Sequence();
            var pointer = Instantiate(_pointerPrefab, _uiManager.transform);
            pointer.transform.position = startPos;
            pointer.enabled = false;
            seq.AppendCallback(() =>
            {
                pointer.enabled = true;
            });
            seq.Append(pointer.transform.DOMove(endPos, 2));
            seq.AppendCallback(() =>
            {
                pointer.enabled = false;
            });
            seq.SetEase(Ease.Linear);
            seq.onKill += () =>
            {
                Destroy(pointer.gameObject);
            };
            return seq;
        }
        
        
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}