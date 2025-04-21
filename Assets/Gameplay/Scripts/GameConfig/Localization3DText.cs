using System;
using Cysharp.Threading.Tasks;
using Gameplay.Scripts.Utils;
using TMPro;
using UnityEngine;
using Zenject;

namespace Gameplay.Configs
{
    [RequireComponent(typeof(TextMeshPro))]
    public class Localization3DText : MonoBehaviour
    {
        [SerializeField] private string _key;
        
        private LocalizationManager _localizationManager;
        private TextMeshPro _text;
        
        [Inject]
        private void Construct(LocalizationManager localizationManager)
        {
            _localizationManager = localizationManager;
        }

        private void Awake()
        {
            _text = GetComponent<TextMeshPro>();
        }
        private async void OnEnable()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            if (_key != String.Empty)
            {
                SetCurrentText();
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
            _text.text = _localizationManager.TryTranslate(_key);
        }

        private void OnDisable()
        {
            _localizationManager.ChangeLanguageAction -= SetCurrentText;
        }
    }
}