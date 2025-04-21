using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.Huds.Scripts
{
    public abstract class HudBase : MonoBehaviour
    {
        public abstract void Init();
        protected abstract UniTask  AnimationIn();
        protected abstract UniTask  AnimationOut();
    }
}