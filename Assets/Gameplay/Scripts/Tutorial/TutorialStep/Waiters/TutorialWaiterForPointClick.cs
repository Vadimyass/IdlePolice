using Gameplay.Scripts.Signals;
using Gameplay.Scripts.Tutorial;
using UnityEngine;
using Zenject;

namespace Tutorial
{
    public class TutorialWaiterForPointClick : TutorialWaiterBase , ITutorialUpdateObserver
    {
        private SignalBus _signalBus;
        private TutorialRunner _tutorialRunner;
        private ITutorialUpdateLoop _updateLoop;
        private bool _isAwait;

        public override bool Process()
        {
            WaiterObject.SetState(TutorialWaiterState.Block, _tutorialRunner.NextStep);
            _isAwait = true;
            _updateLoop.AddTutorialObserver(this);
            _signalBus.Subscribe<TutorialTouch>(OnPointClick);
            return true;
        }

        public override void FinalizeStep()
        {
            _signalBus.TryUnsubscribe<TutorialTouch>(OnPointClick);
            base.FinalizeStep();
        }

        private void OnPointClick()
        {
            _isAwait = false;
        }
        
        public override void Init(TutorialServiceLocator tutorialServiceLocator, TutorialRunner tutorialRunner)
        {
            base.Init(tutorialServiceLocator, tutorialRunner);
            _tutorialRunner = tutorialRunner;
            _signalBus = tutorialServiceLocator.GetService<SignalBus>();
            _updateLoop = tutorialServiceLocator.GetService<ITutorialUpdateLoop>();
        }

        public bool TryUpdate()
        {
            if (_isAwait) return true;
            EndWait();

            return false;
        }
    }
}