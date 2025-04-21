using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.Scripts.UpgradeWindow
{
    public class UpgradeWindowAnimation : UIAnimation
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