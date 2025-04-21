using Gameplay.Scripts.Utils;
using UnityEngine;

namespace Gameplay.Scripts.Tutorial
{
    public abstract class TutorialTargetResolver : MonoBehaviour
    {
        private TutorialTarget _tutorialTarget;
        
        private void Awake()
        {
            BigDDebugger.LogError("AWAKE");
            AddTarget(_tutorialTarget = GetComponent<TutorialTarget>());
        }

        private void OnDestroy()
        {
            RemoveTarget(_tutorialTarget);
        }

        protected abstract void AddTarget(TutorialTarget tutorialTarget);
        protected abstract void RemoveTarget(TutorialTarget tutorialTarget);
    }
}