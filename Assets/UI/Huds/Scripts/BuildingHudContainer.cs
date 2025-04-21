using System;
using Agents;
using BigD.Config;
using Cysharp.Threading.Tasks;
using Gameplay.Configs;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.TimeManagement;
using Gameplay.Scripts.Utils;
using Pool;
using SolidUtilities.Collections;
using TMPro;
using UI.Huds.Scripts.BuildingHuds;
using UI.Scripts.OfficersWindow;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace UI.Huds.Scripts
{
    public class BuildingHudContainer : MonoBehaviour
    {
       [SerializeField] private BuildingIncomeProgressBar buildingIncomeProgressBar;
       [SerializeField] private BuildingIncomeText buildingIncomeText;
       [SerializeField] private OfficerCard _officerCard;
       [SerializeField] private Transform _plusForOfficer;
       [SerializeField] private TextMeshProUGUI _orderCountText;
       [SerializeField] private Image _image;
       [SerializeField] private SerializableDictionary<OfficerType, Transform> _spritesTypes;
       [SerializeField] private BuildingHudArrow _inArrow;
       [SerializeField] private BuildingHudArrow _outArrow;
       
       private PlayerPrefsSaveManager _playerPrefsSaveManager;
       private SpritesConfig _spritesConfig;
       private UIManager _uiManager;
       private GameConfig _gameConfig;

       [Inject]
       private void Construct(GameConfig gameConfig, PlayerPrefsSaveManager playerPrefsSaveManager, SpritesConfig spritesConfig, UIManager uiManager)
       {
           _playerPrefsSaveManager = playerPrefsSaveManager;
           _spritesConfig = spritesConfig;
           _uiManager = uiManager;
           _gameConfig = gameConfig;
       }
       
        public async void Configurate(string officerKey, double incomeProgress, double income,UnityAction takeIncomeAction, SpriteName sprite, OfficerType officerType)
        {
            buildingIncomeProgressBar.Configurate(incomeProgress);
            buildingIncomeText.Configurate(income, takeIncomeAction);
            
            foreach (var spritesType in _spritesTypes)       
            {
                spritesType.Value.gameObject.SetActive(false);
            }
            
            _spritesTypes[officerType].gameObject.SetActive(true);
            
            _officerCard.Init(_spritesConfig, _playerPrefsSaveManager, _uiManager);
            SetOfficer(officerKey);
            SetSprite(sprite);
            SetQueueCount(0, false);
        }

        public void SetSprite(SpriteName sprite)
        {
            _image.sprite = _spritesConfig.GetSpriteByName(sprite);
        }
        
        public void SetOfficer(string officerKey)
        {
            _plusForOfficer.gameObject.SetActive(true);
            _officerCard.gameObject.SetActive(false);

            if (officerKey != String.Empty)
            {
                _plusForOfficer.gameObject.SetActive(false);
                _officerCard.gameObject.SetActive(true);
                var info = _gameConfig.OfficersInfoConfig.GetItemByKey(officerKey);
                _officerCard.Show(info);
            }
        }

        public void SetProgressBarValueNoAnimation(float value)
        {
            buildingIncomeProgressBar.UpdateInfo(value);
        }
        
        public void UpdateInfo(double incomeProgress, double income, int orderCount, bool withUpdateMoney, bool withDelay = false)
        {
            SetQueueCount(orderCount);
            buildingIncomeProgressBar.UpdateInfo(incomeProgress, withDelay);

            if (withUpdateMoney == true)
            {
                UpdateMoney(income, withDelay);
            }
        }

        public void UpdateMoney(double income, bool withDelay = false)
        {
            buildingIncomeText.UpdateInfo(income, withDelay);
        }

        public async UniTask AddOrderUnit(CarAgent carTransform)
        {
            await _inArrow.StartInAnimation(carTransform);
        }
        
        public async UniTask RemoveOrderUnit(Transform carTransform)
        {
            await _outArrow.StartOutAnimation(carTransform);
        }
        
        public async void SetQueueCount(int orderCount, bool withAwait = true)
        {
            if (withAwait == true)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1));
            }
            
            if (orderCount == 0)
            {
                _orderCountText.transform.parent.parent.parent.gameObject.SetActive(false);
                return;
            }
            _orderCountText.transform.parent.parent.parent.gameObject.SetActive(true);
            _orderCountText.text = orderCount.ToString();
        }
        
        public void Return()
        {
            buildingIncomeProgressBar.Hide();
        }

        public void Release()
        {
            buildingIncomeProgressBar.Hide();
        }
    }
}