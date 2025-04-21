using Gameplay.Scripts.Tutorial;
using Gameplay.Scripts.Utils;
using Signals;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Tutorial
{
    public class TutorialWaitForScreenClosed<T> : TutorialWaiterBase, ITutorialUpdateObserver where T: UIScreen
    {
        private ITutorialUpdateLoop _updateLoop;

        private TutorialRunner _tutorialRunner;
        private SignalBus _signalBus;

        private bool _isSignalAwait = true;
        private UIScreenChangeStateSignal.State _waitState = UIScreenChangeStateSignal.State.Hided;

        public TutorialWaitForScreenClosed<T> SetWaitStartScreenHide()
        {
            _waitState = UIScreenChangeStateSignal.State.StartHide;
            return this;
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
            _signalBus.Subscribe<UIScreenChangeStateSignal>(OnWaitSignal);
            
            return true;
        }

        public bool TryUpdate()
        {
            if (_isSignalAwait) return true;
            
            EndWait();

            return false;
        }

        public override void FinalizeStep()
        {
            _signalBus.TryUnsubscribe<UIScreenChangeStateSignal>(OnWaitSignal);
            _updateLoop.RemoveTutorialObserver(this);
            base.FinalizeStep();
        }

        private void OnWaitSignal(UIScreenChangeStateSignal obj)
        {
            if (obj.ScreenType == typeof(T) && obj.UIScreenState == _waitState)
            {
                _isSignalAwait = false;
            }
            TryUpdate();
        }
    }
}