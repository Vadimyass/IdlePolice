using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.Scripts.ChoseLevelWindow
{
    public class ChoseLevelWindowAnimation : UIAnimation
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