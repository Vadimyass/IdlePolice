using Gameplay.Scripts.Tutorial;

namespace Tutorial
{
    public class TutorialChain : TutorialStepBase
    {
        private readonly TutorialStepBase[] _chain;

        public TutorialChain(TutorialStepBase[] chain)
        {
            _chain = chain;
        }

        public override void Init(TutorialServiceLocator tutorialServiceLocator, TutorialRunner tutorialRunner)
        {
            base.Init(tutorialServiceLocator, tutorialRunner);
            foreach (var stepBase in _chain)
            {
                stepBase.Init(tutorialServiceLocator, tutorialRunner);
            }
        }

        public override bool Process()
        {
            for (var i = 0; i < _chain.Length; i++)
            {
                _chain[i].Process();
            }

            return true;
        }

        public override void FinalizeStep()
        {
            foreach (var stepBase in _chain)
            {
                stepBase.FinalizeStep();
            }
        }
    }
}