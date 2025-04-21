using System.Collections;
using UnityEngine;

namespace Gameplay.Scripts.Tutorial
{
    public abstract class TutorialContextTargetFollowerObject : TutorialContextMonoBehaviourObject
    {
        private Coroutine _followTargetRoutine;
        
        protected virtual void OnDisable()
        {
            StopFollow();
        }
        
        protected void StartFollowTarget(TutorialTarget target)
        {
            if (_followTargetRoutine != null)
            {
                StopCoroutine(_followTargetRoutine);
            }

            _followTargetRoutine = StartCoroutine(StartFollowTargetRoutine(target));
        }
        
        IEnumerator StartFollowTargetRoutine(TutorialTarget target)
        {
            while (gameObject.activeSelf)
            {
                if (target == null || !target.gameObject.activeInHierarchy)
                {
                    break;
                }

                UpdateByTarget(target);
                yield return null;
            }

            gameObject.SetActive(false);
        }

        protected abstract void UpdateByTarget(TutorialTarget target);

        public void StopFollow()
        {
            if (_followTargetRoutine != null)
            {
                StopCoroutine(_followTargetRoutine);
                _followTargetRoutine = null;
            }
        }
    }
}