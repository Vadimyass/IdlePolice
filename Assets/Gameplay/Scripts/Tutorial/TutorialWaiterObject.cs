using System;

namespace Gameplay.Scripts.Tutorial
{
    public class TutorialWaiterObject
    {
        public TutorialWaiterState WaiterState { get; private set; } = TutorialWaiterState.None;

        private Action _onStateChangeAction;

        public void SetState(TutorialWaiterState newState, Action onStateChange = null)
        {
            WaiterState = newState;
            var cachedAction = _onStateChangeAction; 
            _onStateChangeAction = onStateChange;
            cachedAction?.Invoke();
        }

        public void Clear()
        {
            WaiterState = TutorialWaiterState.None;
            _onStateChangeAction = null;
        }
    }
    
    public enum TutorialWaiterState { None, Block, Parallel, StartNextInstant, EndInstant }
}