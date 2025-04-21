using System.Collections.Generic;
using Audio;
using Gameplay.Configs;
using Gameplay.Scripts.Buildings;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.LevelManagement;
using Gameplay.Scripts.Locker;
using TMPro;
using UI.UIUtils;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using IPoolable = Pool.IPoolable;

namespace UI.Scripts.UpgradeWindow
{
    public class UpgradeView : MonoBehaviour, IPoolable
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _descrText;
        [SerializeField] private List<TextMeshProUGUI> _costTexts;
        [SerializeField] private Image _image;
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _notEnoughButton;
        private bool _isInited;
        private PlayerPrefsSaveManager _prefsSaveManager;
        public BaseUpgradeConfigInitializator BaseUpgradeConfigInitializator { get; private set; }
        private SignalBus _signalBus;
        private BaseUpgradesController _baseUpgradesController;
        private SpritesConfig _spritesConfig;
        private LevelController _levelController;
        private AudioManager _audioManager;
        private LockController _lockController;

        public void Init(PlayerPrefsSaveManager prefsSaveManager, LockController lockController, AudioManager audioManager,SignalBus signalBus, LevelController levelController, BaseUpgradesController baseUpgradesController, SpritesConfig spritesConfig)
        {
            _lockController = lockController;
            _audioManager = audioManager;
            _levelController = levelController;
            _spritesConfig = spritesConfig;
            _baseUpgradesController = baseUpgradesController;
            if(_isInited == true) return;

            _isInited = true;
            _signalBus = signalBus;
            _prefsSaveManager = prefsSaveManager;
        }

        public void Show(BaseUpgradeConfigInitializator baseUpgradeConfigInitializator)
        {
            BaseUpgradeConfigInitializator = baseUpgradeConfigInitializator;
            _signalBus.Subscribe<ChangeCurrencySignal>(RefreshInfo);
            _buyButton.onClick.RemoveAllListeners();
            _notEnoughButton.onClick.RemoveAllListeners();
            _buyButton.onClick.AddListener(BuyUpgrade);
            _notEnoughButton.onClick.AddListener(BuyUpgrade);
            _nameText.text = BaseUpgradeConfigInitializator.Name;
            _descrText.text = BaseUpgradeConfigInitializator.Description;
            _image.sprite = _spritesConfig.GetSpriteByName(baseUpgradeConfigInitializator.SpriteName);
            
            RefreshInfo();
            foreach (var costText in _costTexts)
            {
                costText.text = TextMeshProUtils.ConvertBigDoubleToText(BaseUpgradeConfigInitializator.Cost);
            }
        }

        private void RefreshInfo()
        {
            var isEnoughMoney = _prefsSaveManager.PrefsData.CurrenciesModel.BasesMoney[_levelController.CurrentLevel.Level] >=
                                BaseUpgradeConfigInitializator.Cost;
            
            _buyButton.gameObject.SetActive(isEnoughMoney);
            _notEnoughButton.gameObject.SetActive(!isEnoughMoney);
        }
        
        private void BuyUpgrade()
        {
            if(_lockController.HaveLock<UpgradeBaseLocker>() && BaseUpgradeConfigInitializator.Key != "Criminals + 3_1") return;
            
            if (_prefsSaveManager.PrefsData.CurrenciesModel.TrySpendMoney(BaseUpgradeConfigInitializator.Cost, _levelController.CurrentLevel.Level))
            {
                _prefsSaveManager.PrefsData.BaseUpgradesModel.AddAsBought(BaseUpgradeConfigInitializator.Key, _levelController.CurrentLevel.Level);
                _baseUpgradesController.AddUpgrade(BaseUpgradeConfigInitializator.Key);
                _prefsSaveManager.ForceSave();
                _audioManager.PlaySound(TrackName.UpgradeButton);
            }
            else
            {
                _signalBus.Fire(new NotEnoughCurrencySignal(CurrencyUIType.Dollar));
            }
        }

        public void Return()
        {
            _buyButton.onClick.RemoveAllListeners();
            _signalBus.TryUnsubscribe<ChangeCurrencySignal>(RefreshInfo);
        }

        public void Release()
        {
            _buyButton.onClick.RemoveAllListeners();
            _signalBus.TryUnsubscribe<ChangeCurrencySignal>(RefreshInfo);
        }
    }
}