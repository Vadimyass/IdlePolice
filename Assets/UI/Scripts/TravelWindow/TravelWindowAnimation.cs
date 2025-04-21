using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.Scripts.TravelWindow
{
    public class TravelWindowAnimation : UIAnimation
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