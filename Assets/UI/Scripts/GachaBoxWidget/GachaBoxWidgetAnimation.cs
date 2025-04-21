using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.Scripts.GachaBoxWidget
{
    public class GachaBoxWidgetAnimation : UIAnimation
    {
        public override UniTask ShowAnimation()
        {
            return UniTask.CompletedTask;
        }

        public override UniTask HideAnimation()
        {
            return UniTask.CompletedTask;
        }
    }
}