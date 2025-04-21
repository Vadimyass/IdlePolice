using System;
using Gameplay.Scripts.Tutorial;
using UnityEngine.Events;

namespace Tutorial
{
    public class TutorialWaitForUnityEvent<T>  : TutorialWaiterBase, ITutorialUpdateObserver
    {
        private ITutorialUpdateLoop _updateLoop;
        private TutorialRunner _tutorialRunner;
        private UnityEvent<T> _unityEvent;
        private readonly Func<T, bool> _predicat;
        private bool _isSignalAwait = true;

        public TutorialWaitForUnityEvent(UnityEvent<T> unityEvent, Func<T, bool> predicat = null)
        {
            _unityEvent = unityEvent;
            _predicat = predicat;
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

            _isSignalAwait = true;
            _unityEvent.AddListener(OnWaitSignal);

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
            _unityEvent.RemoveListener(OnWaitSignal);
            _updateLoop.RemoveTutorialObserver(this);
            base.FinalizeStep();
        }
    }
}