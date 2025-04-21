using System;

namespace UnityEngine.UI
{
    public class UIScreenChangeStateSignal
    {
        public readonly State UIScreenState;
        public readonly Type ScreenType;

        public UIScreenChangeStateSignal(State state, Type screenType)
        {
            UIScreenState = state;
            ScreenType = screenType;
        }
        
        public enum State
        {
            StartShow, Showed, StartHide, Hided 
        }
    }
}