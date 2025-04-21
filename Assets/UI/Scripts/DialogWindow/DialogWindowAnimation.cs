using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.Scripts.DialogWindow
{
    public class DialogWindowAnimation : UIAnimation
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