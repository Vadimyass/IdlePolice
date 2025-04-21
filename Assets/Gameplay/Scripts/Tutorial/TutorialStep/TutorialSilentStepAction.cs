using System;
using Gameplay.Scripts.Tutorial;

namespace Tutorial
{

    public class TutorialSilentStepAction : TutorialStepAction
    {
        public TutorialSilentStepAction(Action action) : base(action)
        {
        }

        public TutorialSilentStepAction AppendAction(Action action)
        {
            Action += action ?? throw new ArgumentNullException(nameof(action));
            return this;
        }

        public TutorialSilentStepAction RemoveAction(Action action)
        {
            Action -= action;
            return this;
        }

        private TutorialWaiterObject _waiterObject;

        public override void Init(TutorialServiceLocator tutorialServiceLocator, TutorialRunner tutorialRunner)
        {
            base.Init(tutorialServiceLocator, tutorialRunner);
            _waiterObject = tutorialServiceLocator.GetService<TutorialWaiterObject>();
        }

        public override bool Process()
        {
            base.Process();
            _waiterObject.SetState(TutorialWaiterState.StartNextInstant);
            return true;
        }
    }
}