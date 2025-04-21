using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.Scripts.OfferWindow
{
    public class OfferWindowAnimation : UIAnimation
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