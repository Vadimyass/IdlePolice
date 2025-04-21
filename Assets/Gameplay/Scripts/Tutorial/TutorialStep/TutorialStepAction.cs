using System;
using Gameplay.Scripts.Tutorial;

namespace Tutorial
{
    public class TutorialStepAction : TutorialStepBase
    {
        protected Action Action { get; set; }

        public TutorialStepAction(Action action)
        {
            Action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public override bool Process()
        {
            Action.Invoke();
            return true;
        }
    }
}