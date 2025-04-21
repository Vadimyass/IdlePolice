using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.Scripts.UpgradeBuildingWindow
{
    public class UpgradeBuildingWindowAnimation : UIAnimation
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