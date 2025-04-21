using UnityEngine;
using UnityEngine.Playables;

namespace Gameplay.Playables.CustomPlayableTracks
{
    [System.Serializable]
    public class WaitForInputPlayable : PlayableBehaviour
    {
        private bool waitingForInput = true;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (waitingForInput && IsScreenTouched())
            {
                waitingForInput = false;
                playable.GetGraph().GetRootPlayable(0).SetSpeed(1); // Продолжить TimeLine
            }
        }

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            waitingForInput = true;
            playable.GetGraph().GetRootPlayable(0).SetSpeed(0); // Остановить TimeLine
        }

        private bool IsScreenTouched()
        {
#if UNITY_EDITOR
            return Input.GetMouseButtonDown(0);
#else
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
#endif
        }
    }
}