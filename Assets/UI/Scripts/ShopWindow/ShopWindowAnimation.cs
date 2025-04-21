using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.Scripts.ShopWindow
{
    public class ShopWindowAnimation : UIAnimation
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