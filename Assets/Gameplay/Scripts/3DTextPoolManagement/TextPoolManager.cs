using DG.Tweening;
using Pool;
using TMPro;
using UnityEngine;

namespace Gameplay.Scripts._3DTextPoolManagement
{
    public class TextPoolManager : MonoBehaviour
    {
        [SerializeField] private MonoBehaviourPool<TextMeshPro> _textPool;

        [SerializeField] private float fadeDuration = 2f;
        [SerializeField] private Vector3 moveOffset = new Vector3(3f, 10f, 0);

        public void GetText(string text,Vector3 position)
        {
            var textObject = _textPool.GetObject();
            textObject.DOFade(1, 0);
            textObject.transform.localScale = Vector3.zero;
            textObject.transform.position = position;
            textObject.text = text;
            
            
            Sequence animationSequence = DOTween.Sequence();
            animationSequence.Append(textObject.transform.DOMove(position + moveOffset, fadeDuration)) // Move up
                .Join(textObject.DOFade(0f, fadeDuration).SetEase(Ease.InQuart)) // Fade out
                .Join(textObject.DOScale(1,1).SetEase(Ease.Linear))
                .OnComplete(() => _textPool.ReturnObject(textObject));
        }
    }
}