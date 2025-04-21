using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.Scripts.OfficerChoiceWidget
{
    public class OfficerChoiceWidgetAnimation : UIAnimation
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