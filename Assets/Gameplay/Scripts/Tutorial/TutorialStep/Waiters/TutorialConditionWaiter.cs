using Gameplay.Scripts.Tutorial;

namespace Tutorial
{
    public class TutorialConditionWaiter : TutorialWaiterBase, ITutorialUpdateObserver
    {
        private ITutorialUpdateLoop _updateLoop;
        private readonly System.Func<bool> _condition;

        private TutorialRunner _tutorialRunner;
        
        public TutorialConditionWaiter(System.Func<bool> condition = null)
        {
            _condition = condition;
        }

        public override void Init(TutorialServiceLocator tutorialServiceLocator, TutorialRunner tutorialRunner)
        {
            base.Init(tutorialServiceLocator, tutorialRunner);
            _updateLoop = tutorialServiceLocator.GetService<ITutorialUpdateLoop>();
            _tutorialRunner = tutorialRunner;
        }

        public override bool Process()
        {
            WaiterObject.SetState(TutorialWaiterState.Block, _tutorialRunner.NextStep);
            _updateLoop.AddTutorialObserver(this);
            return true;
        }

        public bool TryUpdate()
        {
            if(!_condition.Invoke()) return true;
            EndWait();

            return false;
        }
    }
}