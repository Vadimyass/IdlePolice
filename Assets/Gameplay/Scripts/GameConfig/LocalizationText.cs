using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Gameplay.Configs
{
    public class LocalizationText : MonoBehaviour
    {
        [SerializeField] private string _key;
        
        private LocalizationManager _localizationManager;
        protected TextMeshProUGUI _textMeshPro;

        [Inject]
        private void Construct(LocalizationManager localizationManager)
        {
            _localizationManager = localizationManager;
        }

        private void Awake()
        {
            _textMeshPro = GetComponent<TextMeshProUGUI>();
        }
        private void OnEnable()
        {
            if (_key != String.Empty)
            {
                SetCurrentText();
            }
            if (_localizationManager == null)
            {
                return;
            }
            _localizationManager.ChangeLanguageAction += SetCurrentText;
        }

        public void ChangeKey(string key)
        {
            _key = key;
            SetCurrentText();
        }
        
        protected virtual void SetCurrentText()
        {
            return;
            if (_localizationManager == null)
            {
                return;
            }
            _textMeshPro.text = _localizationManager.TryTranslate(_key);
        }

        private void OnDisable()
        {
            if(_localizationManager == null) return;
            
            _localizationManager.ChangeLanguageAction -= SetCurrentText;
        }
    }
}