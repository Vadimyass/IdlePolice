using System;
using BigD.Config;
using Gameplay.Configs;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.LevelManagement;
using Gameplay.Scripts.Signals;
using Gameplay.Scripts.Utils;
using MyBox;
using TMPro;
using UI.Scripts.MainScreen;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Scripts.ShopWindow
{
    public class TimeMachineOffer : MonoBehaviour
    {
        /*[SerializeField] private TimeMachineType _timeMachineType;
        [SerializeField] private TimeMachineTime _timeMachineTime;
        [SerializeField] private TextMeshProUGUI _bonusAfterSkipText;
        [SerializeField] private Button _useButton;
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _notEnoughButton;
        [SerializeField] private Image _image;
        [SerializeField] private int _cost;
        [SerializeField] private TextMeshProUGUI _countText;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private TextMeshProUGUI _costText2;
        [SerializeField, SearchableEnum] private EconomyEnum _configKey;
        private PlayerPrefsSaveManager _playerPrefsSaveManager;
        private LevelController _militaryBaseController;
        //private IncomeController _incomeController;
        private GameConfig _gameConfig;
        private int _bonus;
        private UIManager _uiManager;
        private SignalBus _signalBus;

        [Inject]
        private void Construct(PlayerPrefsSaveManager playerPrefsSaveManager, SignalBus signalBus, UIManager uiManager, GameConfig gameConfig, MilitaryBaseController militaryBaseController)
        {
            _signalBus = signalBus;
            _uiManager = uiManager;
            _gameConfig = gameConfig;
           // _incomeController = incomeController;
            _militaryBaseController = militaryBaseController;
            _playerPrefsSaveManager = playerPrefsSaveManager;
            _signalBus.Subscribe<RefreshShopSignal>(Refresh);
        }

        public void Init()
        {
            _useButton.onClick.RemoveAllListeners();
            _buyButton.onClick.RemoveAllListeners();
            
            _useButton.onClick.AddListener(UseMachine);
            _buyButton.onClick.AddListener(BuyMachine);
            _notEnoughButton.onClick.AddListener(NotEnoughCrystals);
            
            _cost = _gameConfig.EconomyConfig.GetItemByKey(_configKey, _militaryBaseController.CurrentBase.ID);
            
            _costText.text = _cost.ToString();
            _costText2.text = _cost.ToString();
        }

        public void Refresh()
        {
            var count =_playerPrefsSaveManager.PrefsData.ConsumablesInfoModel.GetCountOfTimeMachine(_timeMachineType,
                _timeMachineTime);

            _bonus = CalculateBonus();

            _bonusAfterSkipText.text = _bonus <= 0 ? _bonus.ToString() : "+" + _bonus.ToString();
            
            if (count > 0)
            {
                _useButton.gameObject.SetActive(true);
                _buyButton.gameObject.SetActive(false);
            }
            else
            {
                _useButton.gameObject.SetActive(false);
                _buyButton.gameObject.SetActive(true);
            }

            _countText.text = count.ToString();
            
            
            _notEnoughButton.gameObject.SetActive(_playerPrefsSaveManager.PrefsData.CurrenciesInfoModel.CrystalCount < _cost);

            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)_costText.transform.parent);
            Canvas.ForceUpdateCanvases();
        }

        private async void UseMachine()
        {
            _useButton.interactable = false;
            var args = new TimeMachineWindowArguments(CalculateBonus(), _timeMachineTime, _timeMachineType, _image.sprite);
            await _uiManager.Show<TimeMachineUseWidgetController>(args);
            _useButton.interactable = true;
        }

        private void BuyMachine()
        {
            if (_playerPrefsSaveManager.PrefsData.CurrenciesInfoModel.TrySpendCrystal(_cost))
            {
                _playerPrefsSaveManager.PrefsData.ConsumablesInfoModel.AddTimeMachine(_timeMachineType, _timeMachineTime, 1);
            }
            Refresh();
        }

        private void NotEnoughCrystals()
        {
            _signalBus.Fire(new NotEnoughCurrencySignal(CurrencyUIType.Crystal));
        }

        private int GetHoursForTimeMachine()
        {
            var time = 1;
            switch (_timeMachineTime)
            {
                case TimeMachineTime.oneHour:
                    time = 1;
                    break;
                case TimeMachineTime.fourHours:
                    time = 4;
                    break;
                case TimeMachineTime.twentyFourHours:
                    time = 24;
                    break;
            }

            return time;
        }
        
        private int CalculateBonus()
        {
            var militaryBase = _militaryBaseController.CurrentBase;
            var compartments = _playerPrefsSaveManager.PrefsData.SoldierPlaceInfoModel.GetCountSoldierPlacesForBaseByType(militaryBase.ID);
            var fullCount = 0;
            var time = GetHoursForTimeMachine();

            switch (_timeMachineType)
            {
                case TimeMachineType.Money:
                    var income = (int)_incomeController.RealIncome > 0 ? (int)_incomeController.RealIncome : 100;
                    fullCount = income * 50 * time;
                    break;
                case TimeMachineType.Soldiers:
                    foreach (var soldierType in militaryBase.AvailableSoldierType)
                    {
                        var daysNeed = _gameConfig.EconomyConfig.GetGraduationDaysBySoldierType(soldierType, militaryBase.ID);
                        var divider = _gameConfig.EconomyConfig.GetItemByKey(EconomyConfig.EconomyEnum.time_machine_soldiers_divide, militaryBase.ID);
                        var count = ((compartments[soldierType] * 2 * 5) / daysNeed) * time / divider;
                        fullCount += count;
                    }
                    break;
            }
            
            return fullCount;
        }*/
    }

    public enum TimeMachineType
    {
        Money,
        Soldiers
    }

    public enum TimeMachineTime
    {
        oneHour,
        fourHours,
        twentyFourHours
    }
}