using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.Scripts.MainScreen
{
    public class MainScreenAnimation : UIAnimation
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