using System;
using Gameplay.Scripts.Tutorial;
using Zenject;

namespace Tutorial
{
    public class TutorialWaitForSignal<T> : TutorialWaiterBase, ITutorialUpdateObserver where T: class
    {
        private ITutorialUpdateLoop _updateLoop;

        private TutorialRunner _tutorialRunner;
        private SignalBus _signalBus;
        private readonly Func<T, bool> _predicat;

        private bool _isSignalAwait = true;

        public TutorialWaitForSignal(Func<T, bool> predicat = null)
        {
            _predicat = predicat;
        }
        public override void Init(TutorialServiceLocator tutorialServiceLocator, TutorialRunner tutorialRunner)
        {
            base.Init(tutorialServiceLocator, tutorialRunner);
            _updateLoop = tutorialServiceLocator.GetService<ITutorialUpdateLoop>();
            _signalBus = tutorialServiceLocator.GetService<SignalBus>();
            _tutorialRunner = tutorialRunner;
        }

        public override bool Process()
        {
            WaiterObject.SetState(TutorialWaiterState.Block, _tutorialRunner.NextStep);
            _updateLoop.AddTutorialObserver(this);

            _isSignalAwait = true;
            _signalBus.Subscribe<T>(OnWaitSignal);
            
            return true;
        }

        private void OnWaitSignal(T obj)
        {
            if (_predicat != null && !_predicat(obj)) return;
            
            _isSignalAwait = false;
            TryUpdate();
        }

        public bool TryUpdate()
        {
            if (_isSignalAwait) return true;
            
            EndWait();

            return false;
        }
        
        public override void FinalizeStep()
        {
            _signalBus.TryUnsubscribe<T>(OnWaitSignal);
            _updateLoop.RemoveTutorialObserver(this);
            base.FinalizeStep();
        }
    }
}