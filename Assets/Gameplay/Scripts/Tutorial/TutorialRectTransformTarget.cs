using UnityEngine;

namespace Gameplay.Scripts.Tutorial
{
    public class TutorialRectTransformTarget : TutorialTarget
    {
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = (RectTransform) transform;
        }

        public override Vector2 CalculateSize() => _rectTransform.rect.size * _rectTransform.localScale;

        public override Vector3 CalculatePosition()
        {
            var v = new Vector3[4];

            _rectTransform.GetWorldCorners(v);

            var centerPos = Vector3.zero;

            centerPos.x = (v[0].x + v[2].x) / 2;
            centerPos.y = (v[0].y + v[2].y) / 2;

            return centerPos;
        }
    }
}