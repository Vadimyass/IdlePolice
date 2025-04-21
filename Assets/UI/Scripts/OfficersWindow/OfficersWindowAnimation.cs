using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.Scripts.OfficersWindow
{
    public class OfficersWindowAnimation : UIAnimation
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