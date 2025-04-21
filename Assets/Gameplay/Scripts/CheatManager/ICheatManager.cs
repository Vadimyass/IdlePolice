using System;

namespace Gameplay.Scripts.CheatManager
{
    public interface ICheatManager
    {
        void AddCheat<T>(Action<T> action) where T : CheatItemBase;
        void AddPopup<T>(Action<T> action, string buttonName) where T : CheatPopupBase;

        void Show();
        void Close();
    }
}