using System.Collections.Generic;
using Gameplay.Configs;
using MyBox;
using SolidUtilities.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Scripts.OfficerChoiceWidget
{
    public class OfficerChoiceListsButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Transform _unactiveTransform;
        [SerializeField] private SerializableDictionary<OfficerType, Transform> _itemsByType;
        private bool _isOn;
        private bool _isCanBeClicked;

        public void Init(bool isOn, UnityAction action, List<OfficerType> officerTypes)
        {
            _button.onClick.RemoveAllListeners();
            _isOn = isOn;

            _itemsByType.ForEach(x => x.Value.gameObject.SetActive(false));
            
            foreach (var officerType in officerTypes)
            {
                _itemsByType[officerType].gameObject.SetActive(true);
            }
            
            _button.gameObject.SetActive(_isOn);
            _unactiveTransform.gameObject.SetActive(!_isOn);
            
            _button.onClick.AddListener((action));
        }

        public void ChangeState()
        {
            _isOn = !_isOn;
            _button.gameObject.SetActive(_isOn);
            _unactiveTransform.gameObject.SetActive(!_isOn);
        }
    }
}