using Cysharp.Threading.Tasks;

namespace UI.Scripts.GachaBoxOpenWindow
{
    public class GachaBoxOpenWindowAnimation : UIAnimation
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