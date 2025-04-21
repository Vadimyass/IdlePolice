using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.Scripts.LoadingScreen
{
    public class LoadingScreenAnimation : UIAnimation
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