using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Filter comand can be executed only for allowed windows
    /// </summary>
    class UiWindowFilterAvailableWindows : IUiWindowFilter
    {
        private readonly HashSet<Type> _allowedWindows;
    
        public UiWindowFilterAvailableWindows(params Type[] allowedWindows)
        {
            _allowedWindows = new HashSet<Type>(allowedWindows);
        }

        public void TryAddWindowToFilter(Type type)
        {
            if(_allowedWindows.Contains(type)) return;

            _allowedWindows.Add(type);
        }

        public void TryRemoveFilter(Type type)
        {
            if(_allowedWindows.Contains(type) == false) return;

            _allowedWindows.Remove(type);
        }
        
        public bool CanBeExecuted(Type windowType)
        {
            Debug.Log($"[UIStack] processing filter {ToString()}");
            
            return _allowedWindows.Contains(windowType);
        }
    }
}