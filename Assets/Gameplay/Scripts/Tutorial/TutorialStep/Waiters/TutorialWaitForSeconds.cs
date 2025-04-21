using System;
using Cysharp.Threading.Tasks;
using Gameplay.Scripts.Tutorial;

namespace Tutorial
{
    public class TutorialWaitForSeconds : TutorialWaiterBase, ITutorialUpdateObserver
    {
        private ITutorialUpdateLoop _updateLoop;

        private TutorialRunner _tutorialRunner;

        private readonly float _seconds;
        private bool _isAwait = true;

        public TutorialWaitForSeconds(float seconds)
        {
            _seconds = seconds;
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
            StartDelay();
            return true;
        }

        public bool TryUpdate()
        {
            if (_isAwait) return true;
            
            EndWait();

            return false;
        }

        private async void StartDelay()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_seconds));
            _isAwait = false;
            TryUpdate();
        }

        public override void FinalizeStep()
        {
            _updateLoop.RemoveTutorialObserver(this);
            base.FinalizeStep();
        }
    }
}