using BigD.Config;
using Gameplay.Configs;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.Utils;
using SolidUtilities.Collections;
using TMPro;
using UI.Scripts.OfficersWindow;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;
using IPoolable = Pool.IPoolable;

namespace UI.Scripts.OfficerChoiceWidget
{
    public class OfficerChoiceView : MonoBehaviour, IPoolable
    {
        [SerializeField] private OfficerCard _officerCard;
        [SerializeField] private Transform _availableTransform;
        [SerializeField] private Transform _unavailableTransform;
        [SerializeField] private SerializableDictionary<OfficerType, Transform> _itemsByType;
        [SerializeField] private TextMeshProUGUI _autoText;
        [SerializeField] private TextMeshProUGUI _incomeText;
        [SerializeField] private TextMeshProUGUI _income2Text;
        [SerializeField] private Button _assignButton;
        [SerializeField] private TextMeshProUGUI _textType;
        
        private UIManager _uiManager;
        private PlayerPrefsSaveManager _prefsSaveManager;
        private SpritesConfig _spritesConfig;
        private GameConfig _gameConfig;

        public void Init(SpritesConfig spritesConfig, GameConfig gameConfig, PlayerPrefsSaveManager prefsSaveManager, UIManager uiManager)
        {
            _gameConfig = gameConfig;
            _spritesConfig = spritesConfig;
            _prefsSaveManager = prefsSaveManager;
            _uiManager = uiManager;
        }
        
        public void Show(UnityAction<string> unityAction, OfficerInfo info, bool isAvailable, int automateLevel)
        {
            _officerCard.Init(_spritesConfig, _prefsSaveManager, _uiManager);
            
            
            _officerCard.Show(info);
            
            var saveInfo = _prefsSaveManager.PrefsData.OfficersModel.GetSaveInfo(info.Info.Key);

            var income = "x" + _gameConfig.OfficersLevelsInfoConfig.GetItemByKey(info.Info.OfficerRarity, saveInfo.Level)
                .IncomeMultiplier;
            
            _availableTransform.gameObject.SetActive(isAvailable);
            _unavailableTransform.gameObject.SetActive(!isAvailable);

            foreach (var transform1 in _itemsByType)
            {
                transform1.Value.gameObject.SetActive(false);
            }
            
            _itemsByType[info.Info.OfficerType].gameObject.SetActive(true);
            
            _incomeText.text = income;
            _income2Text.text = income;

            _autoText.text = saveInfo.Level >= automateLevel ? "ON" : "OFF";
            
            _assignButton.onClick.RemoveAllListeners();
            _assignButton.onClick.AddListener((() =>
            {
                unityAction(info.Info.Key);
            }));
        }

        public void Return()
        {
            
        }

        public void Release()
        {
           
        }
    }
}