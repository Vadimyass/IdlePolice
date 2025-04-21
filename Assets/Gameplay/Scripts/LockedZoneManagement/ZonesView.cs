using System;
using System.Collections.Generic;
using System.Globalization;
using BigD.Config;
using DG.Tweening;
using Gameplay.Configs;
using Gameplay.OrderManaging;
using Gameplay.Scripts.Buildings;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.LevelManagement;
using Gameplay.Scripts.Locker;
using Gameplay.Scripts.Utils;
using TMPro;
using Tutorial;
using TypeReferences;
using UI.UIUtils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Zenject;

namespace Gameplay.Scripts.LockedZoneManagement
{
    public class ZonesView : MonoBehaviour , IPointerClickHandler
    {
        [SerializeField] private int _districtIndex;
        [SerializeField] private SpriteRenderer _zoneSprite;
        [SerializeField] private List<Building> _buildings;
        [SerializeField] private TextMeshPro _costText;
        [SerializeField] private MeshRenderer _button;
        [SerializeField] private Material _enoughMoneyMaterial;
        [SerializeField] private Material _notEnoughMoneyMaterial;
        [SerializeField] private GameObject _managementPanel;
        
        private GameConfig _gameConfig;
        private PlayerPrefsSaveManager _playerPrefsSaveManager;
        private float _price;
        private LevelController _levelController;
        private SignalBus _signalBus;
        private int CurrentZoneIndex => _playerPrefsSaveManager.PrefsData.LevelInfoModel.GetZoneIndexByLevel(_levelController.CurrentLevel.Level);
        public bool IsOpen { get; private set; }
        public int Index => _districtIndex;
        
        
        [Inject]
        private void Construct(GameConfig gameConfig,  PlayerPrefsSaveManager playerPrefsSaveManager,LevelController levelController, SignalBus signalBus)
        {
            _signalBus = signalBus;
            _levelController = levelController;
            _playerPrefsSaveManager = playerPrefsSaveManager;
            _gameConfig = gameConfig;
            _signalBus.Subscribe<ChangeCurrencySignal>(RefreshInfo);
        }
        public void Init()
        {
            if(IsOpen == true) return;
            
            _price = _gameConfig.EconomyConfig.GetDistrictPriceByIndex(_districtIndex, _levelController.CurrentLevel.Level);

            if (_playerPrefsSaveManager.PrefsData.TutorialModel.IsContextTutorDone(TutorialType.OpenDistrict) == false && _districtIndex == 1)
            {
                _managementPanel.gameObject.SetActive(false);
            }
            
            
            if (_districtIndex <= CurrentZoneIndex)
            {
                IsOpen = true;
                var info = _gameConfig.DotsUnlockConfig.GetItemByKey(_levelController.CurrentLevel.Level, _districtIndex);
                _levelController.CurrentLevel.AddAvailablePointType(info.PointType);
                HideZoneSprite();
                foreach (var building in _buildings)
                {
                    building.TryShowBuildHud();
                }
            }
            
            if ((CurrentZoneIndex + 1) == _districtIndex)
            {
                _costText.text = TextMeshProUtils.GetFormattedNumberText(_price);
                Show();
                return;
            }

            HideHud();
        }

        private async void Show()
        {
            RefreshInfo();
            
            //if(_playerPrefsSaveManager.PrefsData.TutorialModel.IsContextTutorDone(TutorialType.OpenDistrict) == false && _districtIndex == 1) return;
            
            _managementPanel.gameObject.SetActive(true);
            _zoneSprite.gameObject.SetActive(true);
            gameObject.SetActive(true);
            
            var seq = DOTween.Sequence();
            seq.Append(transform.DOScale(Vector3.one, 0.5f));
            await seq.AsyncWaitForCompletion();
        }

        private void HideHud()
        {
            _managementPanel.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        private void HideZoneSprite()
        {
            _zoneSprite.gameObject.SetActive(false);
        }

        private void RefreshInfo()
        {
            _button.material =
                _playerPrefsSaveManager.PrefsData.CurrenciesModel.BasesMoney[_levelController.CurrentLevel.Level] <
                _price
                    ? _notEnoughMoneyMaterial
                    : _enoughMoneyMaterial;
        }

        public async void OnPointerClick(PointerEventData eventData)
        {
            if(_playerPrefsSaveManager.PrefsData.CurrenciesModel.TrySpendMoney(_price,_levelController.CurrentLevel.Level))
            {
                IsOpen = true;
                
                var seq = DOTween.Sequence();
                seq.Append(transform.DOScale(0, 0.5f));
                await seq.AsyncWaitForCompletion();
                
                _playerPrefsSaveManager.PrefsData.LevelInfoModel.SetCurrentZoneIndex(_districtIndex,_levelController.CurrentLevel.Level);
                _playerPrefsSaveManager.ForceSave();
                _zoneSprite.sharedMaterial = new Material(_zoneSprite.sharedMaterial);

                await _zoneSprite.sharedMaterial.DOFloat(1, "_Dissolve", 1.5f).AsyncWaitForCompletion();
                
                _signalBus.Fire(new OpenDistrictZoneSignal(_districtIndex));
                foreach (var building in _buildings)
                {
                    building.TryShowBuildHud();
                }
                
                var info = _gameConfig.DotsUnlockConfig.GetItemByKey(_levelController.CurrentLevel.Level,
                    _districtIndex);
                _levelController.CurrentLevel.AddAvailablePointType(info.PointType);
                HideHud();
                HideZoneSprite();
            }
            else
            {
                _signalBus.Fire(new NotEnoughCurrencySignal(CurrencyUIType.Dollar));
            }
        }
    }
}