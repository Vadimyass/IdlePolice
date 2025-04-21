using System;

namespace UI
{
    public interface IUiWindowFilter
    {
        bool CanBeExecuted(Type windowType);
    }
}