using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    class UiWindowFilterSkipWindows : IUiWindowFilter
    {
        private readonly HashSet<Type> _windowsToSkip;
    
        public UiWindowFilterSkipWindows(params Type[] windowsToSkip)
        {
            _windowsToSkip = new (windowsToSkip);
        }
        
        public bool CanBeExecuted(Type windowType)
        {
            Debug.Log($"[UIStack] processing filter {ToString()}");
            
            return !_windowsToSkip.Contains(windowType);
        }
    }
}