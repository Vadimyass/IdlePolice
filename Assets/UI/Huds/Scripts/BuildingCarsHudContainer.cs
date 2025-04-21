using System;
using System.Collections.Generic;
using BigD.Config;
using Cysharp.Threading.Tasks;
using Gameplay.Configs;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.TimeManagement;
using Gameplay.Scripts.Utils;
using Pool;
using SolidUtilities.Collections;
using UI.Huds.Scripts.BuildingHuds;
using UI.Scripts.OfficersWindow;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Zenject;

namespace UI.Huds.Scripts
{
    public class BuildingCarsHudContainer : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _carsObjects;
       [SerializeField] private OfficerCard _officerCard;
       [SerializeField] private Transform _plusForOfficer;
       [SerializeField] private BuildingIncomeText _buildingIncomeText;
       [SerializeField] private SerializableDictionary<OfficerType, Transform> _spritesTypes;
       
       private GameConfig _gameConfig;
       private UIManager _uiManager;
       private SpritesConfig _spritesConfig;
       private PlayerPrefsSaveManager _playerPrefsSaveManager;

       public BuildingIncomeText BuildingIncomeText => _buildingIncomeText;

       [Inject]
       private void Construct(GameConfig gameConfig, PlayerPrefsSaveManager playerPrefsSaveManager, SpritesConfig spritesConfig, UIManager uiManager)
       {
           _playerPrefsSaveManager = playerPrefsSaveManager;
           _spritesConfig = spritesConfig;
           _uiManager = uiManager;
           _gameConfig = gameConfig;
       }
       
        public async void Configurate(string officerKey, OfficerType officerType, double income, UnityAction takeIncomeAction)
        {
            _plusForOfficer.gameObject.SetActive(true);
            _officerCard.gameObject.SetActive(false);
            _buildingIncomeText.Configurate(income, takeIncomeAction);
            
            foreach (var sprite in _spritesTypes)       
            {
                sprite.Value.gameObject.SetActive(false);
            }
                
            _spritesTypes[officerType].gameObject.SetActive(true);
            
            _officerCard.Init(_spritesConfig, _playerPrefsSaveManager, _uiManager);
            SetOfficer(officerKey, officerType);
        }

        public void SetOfficer(string officerKey, OfficerType officerType)
        {
            _plusForOfficer.gameObject.SetActive(true);
            _officerCard.gameObject.SetActive(false);

            if (officerKey != String.Empty)
            {
                _officerCard.Init(_spritesConfig, _playerPrefsSaveManager, _uiManager);
            
                foreach (var sprite in _spritesTypes)       
                {
                    sprite.Value.gameObject.SetActive(false);
                }
                
                _spritesTypes[officerType].gameObject.SetActive(true);
                
                if (officerKey != String.Empty)
                {
                    _plusForOfficer.gameObject.SetActive(false);
                    _officerCard.gameObject.SetActive(true);
                    var info = _gameConfig.OfficersInfoConfig.GetItemByKey(officerKey);
                    _officerCard.Show(info);
                }
            }
        }
        
        public void SetCarsCount(int count)
        {
            foreach (var carsObject in _carsObjects)
            {
                carsObject.SetActive(false);
            }

            for (int i = 0; i < count; i++)
            {
                _carsObjects[i].SetActive(true);
            }
        }

        public void SetMoneyToMoneyHud(double money)
        {
            _buildingIncomeText.UpdateInfo(money);
        }
    }
}