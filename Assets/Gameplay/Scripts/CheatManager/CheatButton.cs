using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Gameplay.Scripts.CheatManager
{
    public class CheatButton : CheatItemBase
    {
        [SerializeField] private TextMeshProUGUI _buttonLabel;
        [SerializeField] protected Button _button;

        public CheatButton SetButtonName(string buttonName)
        {
            _buttonLabel.text = buttonName;
            return this;
        }

        public CheatButton SetButtonCallback(UnityAction callback,bool forceClose = true)
        {
            _button.onClick.AddListener(callback);
            if (forceClose)
            {
                _button.onClick.AddListener(_cheatManager.Close);
            }
                
            return this;
        }
    }
}