using System;
using System.Linq;
using BigD.Config;
using Gameplay.Configs;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.LevelManagement;
using Gameplay.Scripts.Locker;
using Gameplay.Scripts.Signals;
using Gameplay.Scripts.TimeManagement;
using Gameplay.Scripts.Utils;
using MyBox;
using TMPro;
using UI.Scripts.GachaBoxOpenWindow;
using UI.Scripts.GachaBoxWidget;
using UI.UIUtils;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Scripts.ShopWindow.Gacha
{
    public class GachaBoxView : MonoBehaviour
    {
        [SerializeField] private string _nameKey;
        [SerializeField] private GachaBoxType _gachaBoxType;
        [SerializeField] private Image _image;
        [SerializeField] private Button _useButton;
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _notEnoughButton;
        [SerializeField] private TextMeshProUGUI _countText;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private TextMeshProUGUI _costText2;
        [SerializeField, SearchableEnum] private EconomyEnum _configKey;
        [SerializeField] private bool _isByAd;
        
        private PlayerPrefsSaveManager _playerPrefsSaveManager;
        private LevelController _levelController;
        private GameConfig _gameConfig;
        private int _bonus;
        private UIManager _uiManager;
        private SignalBus _signalBus;
        private int _cost;
        //private AdManager _adManager;
        private Timer _timer;
        private LockController _lockController;

        private const string TimerKey = "GachaAdCooldown";
        
        [Inject]
        private void Construct(PlayerPrefsSaveManager playerPrefsSaveManager, LockController lockController, SignalBus signalBus, UIManager uiManager, GameConfig gameConfig, LevelController levelController)
        {
            _lockController = lockController;
            //_adManager = adManager;
            _signalBus = signalBus;
            _uiManager = uiManager;
            _gameConfig = gameConfig;
            _levelController = levelController;
            _playerPrefsSaveManager = playerPrefsSaveManager;
            _signalBus.Subscribe<RefreshShopSignal>(Refresh);
            _signalBus.Subscribe<ChangeCurrencySignal>(Refresh);
        }

        public void Init()
        {
            _useButton.onClick.RemoveAllListeners();
            _buyButton.onClick.RemoveAllListeners();
            
            _useButton.onClick.AddListener(UseMachine);
            _buyButton.onClick.AddListener(BuyMachine);
            
            _cost = (int)_gameConfig.EconomyConfig.GetItemByKey(_configKey);

            if (_isByAd == false)
            {
                _costText.text = _cost.ToString();
                _costText2.text = _cost.ToString();
                _notEnoughButton.onClick.AddListener(NotEnoughCrystals);
            }
            else
            {
                if (_timer == null)
                {
                    if (_playerPrefsSaveManager.PrefsData.TimerModel.IsExistTimer(TimerKey))
                    {
                        CreateTimer();
                    }
                    else
                    {
                        _buyButton.gameObject.SetActive(true);
                    }
                }
            }
        }

        private void OnTimerEnd(string obj)
        {
            _timer.OnTickAction -= OnTimerTick;
            _timer.OnEndAction -= OnTimerEnd;
            Refresh();
        }

        private void CreateTimer()
        {
            if (_timer != null)
            {
                if (_timer.OnTickAction != null)
                {
                    _timer.OnTickAction -= OnTimerTick;
                }

                if (_timer.OnEndAction != null)
                {
                    _timer.OnEndAction -= OnTimerEnd;
                }
            }

            _playerPrefsSaveManager.PrefsData.TimerModel.TryGetTimer(TimerKey, 3600*1,
                out _timer, -1);
            _costText2.text = TextMeshProUtils.GetTextForTimer(_timer.Duration);
            
            _timer.OnTickAction += OnTimerTick;
            _timer.OnEndAction += OnTimerEnd;
        }

        private void OnTimerTick(float duration)
        {
            _costText2.text = TextMeshProUtils.GetTextForTimer(_timer.Duration);
        }
        
        public void Refresh()
        {
            var count =_playerPrefsSaveManager.PrefsData.ConsumablesInfoModel.GetCountOfGachaBox(_gachaBoxType);
            
            
            if (count > 0)
            {
                _countText.transform.parent.gameObject.SetActive(true);
                _useButton.gameObject.SetActive(true);
                _buyButton.gameObject.SetActive(false);
            }
            else
            {
                _countText.transform.parent.gameObject.SetActive(false);
                _useButton.gameObject.SetActive(false);
                _buyButton.gameObject.SetActive(true);
            }

            _countText.text = count.ToString();

            if (_isByAd == false)
            {
                _notEnoughButton.gameObject.SetActive(
                    _playerPrefsSaveManager.PrefsData.CurrenciesModel.CrystalCount < _cost);
            }
            else
            {
                _buyButton.gameObject.SetActive(true);
                if (_timer != null)
                {
                    if (_timer.Duration > 0 && _timer.IsEnded == false)
                    {
                        _notEnoughButton.gameObject.SetActive(true);
                        _buyButton.gameObject.SetActive(false);
                    }
                    else
                    {
                        _notEnoughButton.gameObject.SetActive(false);
                        _buyButton.gameObject.SetActive(true);
                    }
                }
                else
                {
                    _notEnoughButton.gameObject.SetActive(false);
                    _buyButton.gameObject.SetActive(true);
                }
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)_costText.transform.parent);
            Canvas.ForceUpdateCanvases();
        }

        private async void UseMachine()
        {
            _useButton.interactable = false;
            var count = _playerPrefsSaveManager.PrefsData.ConsumablesInfoModel.GetCountOfGachaBox(_gachaBoxType);
            count = count > 10 ? 10 : count;
            if (_playerPrefsSaveManager.PrefsData.ConsumablesInfoModel.TryConsumeGachaBox(_gachaBoxType, count))
            {
                var args = new GachaBoxOpenWindowArguments(_gachaBoxType, _nameKey, _image.sprite, count);
                await _uiManager.Show<GachaBoxOpenWindowController>(args);
            }

            _useButton.interactable = true;
            
            _playerPrefsSaveManager.ForceSave();
        }

        private void BuyMachine()
        {
            if(_lockController.HaveLock<ShopLocker>()) return; 
            
            if (_isByAd == true)
            {
                _playerPrefsSaveManager.PrefsData.ConsumablesInfoModel.AddGachaBox(_gachaBoxType, 25);
                CreateTimer();
                /*_adManager.ShowRewardedAd((result) =>
                {
                    if (result == false)
                    {
                        return;
                    }
                    

                },AdPlacement.basic_box);*/
            }
            else
            {
                if (_playerPrefsSaveManager.PrefsData.CurrenciesModel.TrySpendCrystal(_cost))
                {
                    _playerPrefsSaveManager.PrefsData.ConsumablesInfoModel.AddGachaBox(_gachaBoxType, 25);
                } 
            }
            Refresh();
        }

        private void NotEnoughCrystals()
        {
            _signalBus.Fire(new NotEnoughCurrencySignal(CurrencyUIType.Crystal));
        }

    }
}