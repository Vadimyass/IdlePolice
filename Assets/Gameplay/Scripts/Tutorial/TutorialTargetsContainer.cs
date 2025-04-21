using System;
using System.Collections.Generic;
using Gameplay.Scripts.Utils;

namespace Gameplay.Scripts.Tutorial
{
    public class TutorialTargetsContainer
    {
        public IReadOnlyList<TutorialTarget> Targets => _targets;
        private List<TutorialTarget> _targets = new List<TutorialTarget>();

        public TutorialTarget GetTarget(Func<TutorialTarget, bool> targetCondition)
        {
            foreach (var target in _targets)
            {
                if (targetCondition(target))
                {
                    return target;
                }
            }

            return null;
        }

        public void Add(TutorialTarget tutorialTarget)
        {
            BigDDebugger.LogError("add", tutorialTarget.name);
            _targets.Add(tutorialTarget);
        }

        public void Remove(TutorialTarget tutorialTarget)
        {
            _targets.Remove(tutorialTarget);
        }
    }
}