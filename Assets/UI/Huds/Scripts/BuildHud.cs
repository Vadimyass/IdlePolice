using System;
using Audio;
using BigD.Config;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Configs;
using Gameplay.Scripts.Agents;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.LevelManagement;
using Gameplay.Scripts.TimeManagement;
using Gameplay.Scripts.Utils;
using Pool;
using SolidUtilities.Collections;
using TMPro;
using UI.Huds.Scripts.BuildingHuds;
using UI.Scripts.OfficersWindow;
using UI.UIUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace UI.Huds.Scripts
{
    public class BuildHud : MonoBehaviour , IPointerClickHandler
    {
        [SerializeField] private Transform _buildButton;
        [SerializeField] private TextMeshPro _costText;
        [SerializeField] private TextMeshPro _buildingNameText;
        [SerializeField] private SerializableDictionary<OfficerType, Transform> _agentTypeDictionary;
        [SerializeField] private MeshRenderer _buttonMeshRenderer;
        [SerializeField] private Material _activeMaterial;
        [SerializeField] private Material _unactiveMaterial;
        [SerializeField] private Collider _buildingCollider;

        private bool _isActive;
        private PlayerPrefsSaveManager _playerPrefsSaveManager;
       private SpritesConfig _spritesConfig;
       private UIManager _uiManager;
       private GameConfig _gameConfig;
       private UnityAction _buildAction;
       private double _cost;
       private LevelController _levelController;
       private SignalBus _signalBus;
       private AudioManager _audioManager;

       [Inject]
       private void Construct(GameConfig gameConfig, AudioManager audioManager, SignalBus signalBus, LevelController levelController, PlayerPrefsSaveManager playerPrefsSaveManager, SpritesConfig spritesConfig, UIManager uiManager)
       {
           _audioManager = audioManager;
           _signalBus = signalBus;
           _levelController = levelController;
           _playerPrefsSaveManager = playerPrefsSaveManager;
           _spritesConfig = spritesConfig;
           _uiManager = uiManager;
           _gameConfig = gameConfig;
       }
       
        public void Show(BuildingConfigInitializator info, UnityAction buildAction)
        {
            transform.gameObject.SetActive(true);
            var money = _playerPrefsSaveManager.PrefsData.CurrenciesModel.BasesMoney[_levelController.CurrentLevel.Level];
            
            _cost = info.Cost;
            _buildingNameText.text = info.Name;

            foreach (var transform1 in _agentTypeDictionary)
            {
                transform1.Value.gameObject.SetActive(false);
            }
            
            _agentTypeDictionary[info.BuildingType].gameObject.SetActive(true);
            
            _costText.text = TextMeshProUtils.GetFormattedNumberText(_cost);
            _buttonMeshRenderer.sharedMaterial = money >= _cost ? _activeMaterial : _unactiveMaterial;

            _buildingCollider.enabled = false;
            
            _buildAction = buildAction;
            _isActive = true;
        }

        public void Hide()
        {
            transform.gameObject.SetActive(false);
            _buildingCollider.enabled = true;
            _isActive = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isActive == false) return;
            if (_playerPrefsSaveManager.PrefsData.CurrenciesModel.TrySpendMoney(_cost,
                    _levelController.CurrentLevel.Level))
            {
                _audioManager.PlaySound(TrackName.BuildBuilding);
                _buildAction();
                Hide();
            }
            else
            {
                _signalBus.Fire(new NotEnoughCurrencySignal(CurrencyUIType.Dollar));
            }
        }
        
        public void OnClickOutside()
        {
            Hide();
        }
    }
}