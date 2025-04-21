using System;
using Audio;
using DG.Tweening;
using Gameplay.Configs;
using Gameplay.Scripts.Buildings;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.LevelManagement;
using Gameplay.Scripts.Utils;
using TMPro;
using UI.UIUtils;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace UI.Scripts.MainScreen
{
    public class CurrencyContainer : MonoBehaviour
    {
        [SerializeField] private CurrencyUIType _currencyType;
        [SerializeField] private Image _currencyImage;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Button _button;
        private SignalBus _signalBus;
        
        private Color _textColor;
        private float _fontSize;
        private AudioManager _audioManager;
        private PlayerPrefsSaveManager _playerPrefsData;
        private LevelController _levelController;
        public CurrencyUIType CurrencyType => _currencyType;
        public Image Image => _currencyImage;

        [Inject]
        private void Construct(SignalBus signalBus, LevelController levelController, AudioManager audioManager, PlayerPrefsSaveManager playerPrefsData)
        {
            _levelController = levelController;
            _playerPrefsData = playerPrefsData;
            _audioManager = audioManager;
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _textColor = _text.color;
            if (_textColor.a == 0)
            {
                _textColor = Color.white;
            }
            _fontSize = _text.fontSize;
        }

        public void Init()
        {
            Refresh();
            _signalBus.Subscribe<ChangeCurrencySignal>(OnCurrencyChange);
            _signalBus.Subscribe<NotEnoughCurrencySignal>(NotEnoughCurrency);
        }


        private void OnCurrencyChange(ChangeCurrencySignal signal)
        {
            if (signal.CurrencyType == _currencyType)
            {
                Refresh();
                
                if (signal.MoneyChange > 0)
                {
                    var seq = DOTween.Sequence();
                    seq.Append(_text.DOFontSize(_fontSize * 1.1f, 0.25f));
                    seq.Join(_text.DOColor(Color.green, 0.25f));
                    seq.Join(_currencyImage.transform.DOScale(1.1f, 0.25f));
                    seq.Append(_text.DOFontSize(_fontSize, 0.25f));
                    seq.Join(_text.DOColor(_textColor, 0.25f));
                    seq.Join(_currencyImage.transform.DOScale(1, 0.25f));
                }
                else if (signal.MoneyChange < 0)
                {
                    var seq = DOTween.Sequence();
                    seq.Append(_text.DOFontSize(_fontSize * 0.9f, 0.25f));
                    seq.Join(_text.DOColor(Color.red, 0.25f));
                    seq.Join(_currencyImage.transform.DOScale(0.9f, 0.25f));
                    seq.Append(_text.DOFontSize(_fontSize, 0.25f));
                    seq.Join(_text.DOColor(_textColor, 0.25f));
                    seq.Join(_currencyImage.transform.DOScale(1, 0.25f));
                }

            }
        }

        public void Refresh()
        {
            switch (_currencyType)
            {
                case CurrencyUIType.Dollar:
                    _text.text = TextMeshProUtils.ConvertBigDoubleToText(_playerPrefsData.PrefsData.CurrenciesModel
                        .BasesMoney[_levelController.CurrentLevel.Level]);
                    break;
                case CurrencyUIType.Donut:
                    _text.text = TextMeshProUtils.ConvertBigDoubleToText(_playerPrefsData.PrefsData.CurrenciesModel.DonutCount);
                    break;
                case CurrencyUIType.Crystal:
                    _text.text = _playerPrefsData.PrefsData.CurrenciesModel.CrystalCount.ToString("0");
                    break;
                default:
                    BigDDebugger.LogError("add new currency realization!!");
                    break;
            }
        }

        private void NotEnoughCurrency(NotEnoughCurrencySignal signal)
        {
            if (signal.CurrencyType == _currencyType)
            {
                _audioManager.PlaySound(TrackName.Anti_Income_Sound);
                var seq = DOTween.Sequence();
                seq.Append(_text.DOFontSize(_fontSize * 1.1f, 0.1f));
                seq.Join(_text.DOColor(Color.red, 0.1f));
                seq.Join(_currencyImage.DOColor(Color.red, 0.1f));
                if (_button != null)
                {
                    seq.Join(_button.transform.DOScale(1.2f, 0.1f));
                }

                seq.Append(_text.DOFontSize(_fontSize, 0.1f));
                seq.Join(_text.DOColor(_textColor, 0.1f));
                seq.Join(_currencyImage.DOColor(Color.white, 0.1f));
                if (_button != null)
                {
                    seq.Join(_button.transform.DOScale(1, 0.1f));
                }
                seq.SetLoops(3);
            }
        }

        private void OnDestroy()
        {
            _signalBus.TryUnsubscribe<ChangeCurrencySignal>(OnCurrencyChange);
            _signalBus.TryUnsubscribe<NotEnoughCurrencySignal>(NotEnoughCurrency);
        }
    }
    
}