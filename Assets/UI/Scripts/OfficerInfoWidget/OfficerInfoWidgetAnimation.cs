using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace UI.Scripts.OfficerInfoWidget
{
    public class OfficerInfoWidgetAnimation : UIAnimation
    {
        public override async UniTask ShowAnimation()
        {
            await _panelTransform.DOScale(1, _animationDuration).From(0).AsyncWaitForCompletion();
        }

        public override async UniTask HideAnimation()
        {
            await _panelTransform.DOScale(0, _animationDuration).From(1).AsyncWaitForCompletion();
        }
    }
}