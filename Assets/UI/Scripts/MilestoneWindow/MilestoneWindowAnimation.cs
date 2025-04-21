using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.Scripts.MilestoneWindow
{
    public class MilestoneWindowAnimation : UIAnimation
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