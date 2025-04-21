using System.Collections.Generic;
using Gameplay.Configs;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.Utils;
using ModestTree;
using SolidUtilities.Collections;
using TMPro;
using UI.Scripts.Basic;
using UI.Scripts.OfficerChoiceWidget;
using UI.Scripts.OfficerInfoWidget;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using IPoolable = Pool.IPoolable;

namespace UI.Scripts.OfficersWindow
{
    public class OfficerCard : MonoBehaviour, IPoolable
    {
        [SerializeField] private Image _officerImage;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private List<SpriteWithRarityStates> _spriteWithRarityStatesList;
        [SerializeField] private SerializableDictionary<OfficerType, Transform> _spritesTypes;
        [SerializeField] private Button _button;
        private PlayerPrefsSaveManager _prefsSaveManager;
        private SpritesConfig _spritesConfig;
        private UIManager _uiManager;

        public void Init(SpritesConfig spritesConfig, PlayerPrefsSaveManager prefsSaveManager, UIManager uiManager)
        {
            _uiManager = uiManager;
            _spritesConfig = spritesConfig;
            _prefsSaveManager = prefsSaveManager;
        }

        public void Show(OfficerConfigInitializator officerInfo)
        {
            var info = new OfficerInfo();
            
            var save = _prefsSaveManager.PrefsData.OfficersModel.GetSaveInfo(officerInfo.Key);

            info.SaveInfo = save;
            info.Info = officerInfo;

            _officerImage.sprite = _spritesConfig.GetSpriteByName(officerInfo.SpriteName);
            
            Show(info);
        }

        public void Show(OfficerInfo officerInfo)
        {
            _button.onClick.RemoveAllListeners();
            foreach (var spriteState in _spriteWithRarityStatesList)
            {
                spriteState.ChangeState(officerInfo.Info.OfficerRarity);
            }
            
            var info = officerInfo.SaveInfo;

            if (_spritesTypes.IsEmpty() == false)
            {
                foreach (var sprite in _spritesTypes)
                {
                    sprite.Value.gameObject.SetActive(false);
                }

                _spritesTypes[officerInfo.Info.OfficerType].gameObject.SetActive(true);
            }

            if (info != null)
            {
                _levelText.text = "<color="+"yellow"+">Lv.</color>" + info.Level;
                _button.onClick.AddListener((() =>
                {
                    var args = new OfficerInfoWidgetArguments(officerInfo.Info.Key);
                    _uiManager.Show<OfficerInfoWidgetController>(args);
                }));
            }
            else
            {
                _button.onClick.AddListener((() =>
                {
                    _prefsSaveManager.PrefsData.OfficersModel.AddOfficer(officerInfo.Info.Key);
                    _prefsSaveManager.ForceSave();
                }));
            }
            
            _officerImage.sprite = _spritesConfig.GetSpriteByName(officerInfo.Info.SpriteName);
        }

        public void Return()
        {
            
        }

        public void Release()
        {
            
        }
    }
}