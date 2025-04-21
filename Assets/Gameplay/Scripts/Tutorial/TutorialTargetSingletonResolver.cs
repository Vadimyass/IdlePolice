using System;

namespace Gameplay.Scripts.Tutorial
{
    public class TutorialTargetSingletonResolver : TutorialTargetResolver
    {
        public static readonly Lazy<TutorialTargetsContainer> Container = new Lazy<TutorialTargetsContainer>(() => new TutorialTargetsContainer());
        
        protected override void AddTarget(TutorialTarget tutorialTarget)
        {
            Container.Value.Add(tutorialTarget);
        }

        protected override void RemoveTarget(TutorialTarget tutorialTarget)
        {
            Container.Value.Remove(tutorialTarget);
        }
    }
}