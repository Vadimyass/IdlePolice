using UnityEngine;

namespace Gameplay.Scripts.Agents
{
    public abstract class AgentAnimation : MonoBehaviour
    {
        public abstract void PlayMoveAnimation();
        public abstract void StopMoveAnimation();
        public abstract void PlayShakeAnimation();
        public abstract void StopShakeAnimation();
    }
}