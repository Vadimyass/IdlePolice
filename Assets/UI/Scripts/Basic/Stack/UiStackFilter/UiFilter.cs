using System;
using System.Collections.Generic;

namespace UI
{
    public class UiFilter
    {
        private readonly List<IUiWindowFilter> _commandShowFilters = new List<IUiWindowFilter>();

        public void AddCreateFilter(IUiWindowFilter commandFilter)
        {
            _commandShowFilters.Add(commandFilter);

        }
        
        public void RemoveCreateFilter(IUiWindowFilter commandFilter)
        {
            _commandShowFilters.Remove(commandFilter);
        }

        public bool CanCreateCommand(Type windowType)
        {
            foreach (var windowFilter in _commandShowFilters)
            {
                if (!windowFilter.CanBeExecuted(windowType))
                {
                    return false;
                }
            }

            return true;
        }
    }
}