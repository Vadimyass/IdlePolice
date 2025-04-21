using System;
using System.Collections.Generic;
using System.Linq;
using Audio;
using BigD.Config;
using Cysharp.Threading.Tasks;
using Gameplay.Configs;
using Gameplay.Scripts.Buildings;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.LevelManagement;
using Gameplay.Scripts.Utils;
using UI.Scripts.ShopWindow;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace UI.Scripts.OfficerChoiceWidget
{
    public class OfficerChoiceWidgetController : UIScreenController<OfficerChoiceWidget>
    {
        private Building _building;
        private PlayerPrefsSaveManager _prefsSaveManager;
        private GameConfig _gameConfig;
        private UIManager _uiManager;
        private SpritesConfig _spritesConfig;
        private UnityAction<string, bool> _action;
        private LevelController _levelController;

        [Inject]
        private void Construct(PlayerPrefsSaveManager prefsSaveManager, LevelController levelController, SpritesConfig spritesConfig, GameConfig gameConfig, UIManager uiManager)
        {
            _levelController = levelController;
            _spritesConfig = spritesConfig;
            _uiManager = uiManager;
            _gameConfig = gameConfig;
            _prefsSaveManager = prefsSaveManager;
        }
        
        public override async UniTask Display(UIArguments arguments)
        {
            await base.Display(arguments);
            var args = (OfficerChoiceWidgetArguments)arguments;

            _action = args.Action;
            View.CloseButton.onClick.RemoveAllListeners();
            View.RecallButton.onClick.RemoveAllListeners();
            View.ShopButton.onClick.RemoveAllListeners();
            
            View.ShopButton.onClick.AddListener((() =>
            {
                _audioManager.PlaySound(TrackName.ButtonClick);
                _uiManager.Show<ShopWindowController>();
            }));
            
            View.RecallButton.onClick.AddListener(RecallOfficer);
            
            _building = args.Building;
            View.BuildingName.text = _building.Info.Name;

            foreach (var viewLvlNeedText in View.LvlNeedTexts)
            {
                viewLvlNeedText.text = "LVL <color=#FFFFFF>" + _building.Info.AutomateLvl +
                                       "</color> officer needed to automate";
            }
            
            
            View.OfficerCard.Init(_spritesConfig, _prefsSaveManager, _uiManager);
            
            if (_building.OfficerSet == string.Empty)
            {
                View.EmptyOfficerInfo.gameObject.SetActive(true);
                View.ChosenOfficerInfo.gameObject.SetActive(false);
            }
            else
            {
                View.EmptyOfficerInfo.gameObject.SetActive(false);
                View.ChosenOfficerInfo.gameObject.SetActive(true);
                var info = _gameConfig.OfficersInfoConfig.GetItemByKey(_building.OfficerSet);
                
                View.OfficerCard.Show(info);
            }

            foreach (var listForAgentType in View.ItemsByTypeForBuilding)
            {
                foreach (var transform in listForAgentType.Value.Transforms)
                {
                    transform.gameObject.SetActive(false);
                }
            }
            
            foreach (var transform in View.ItemsByTypeForBuilding[_building.Info.BuildingType].Transforms)
            {
                transform.gameObject.SetActive(true);
            }
            
            View.SuitableButton.Init(true, GoToAvailableOfficers, new List<OfficerType>(){_building.Info.BuildingType});
            View.NotSuitableButton.Init(false, GoToUnavailableOfficers, Enum.GetValues(typeof(OfficerType)).Cast<OfficerType>().Where(x => x != _building.Info.BuildingType).ToList());
            
            View.CloseButton.onClick.AddListener((() =>
            {
                _audioManager.PlaySound(TrackName.ButtonClick);
                _uiManager.HideLast();
            }));
            GoToAvailableOfficers();

            foreach (var rebuildTransform in View.RebuildTransforms)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(rebuildTransform);
            }
        }

        private void RecallOfficer()
        {
            _audioManager.PlaySound(TrackName.ButtonClick);
            _uiManager.HideLast(); 
            _action(String.Empty, false);
        }

        private void GoToAvailableOfficers()
        {
            View.SuitableButton.ChangeState();
            View.NotSuitableButton.ChangeState();
            
            View.OfficersChoicePool.ReturnAll();
            var autoLevel = _building.Info.AutomateLvl;

            var saveInfos = _prefsSaveManager.PrefsData.OfficersModel.OfficerSaveInfos.ToList();
            var infos = new List<OfficerInfo>();
            
            for (int i = 0; i < saveInfos.Count; i++)
            {
                if (_levelController.CurrentLevel.TryGetBuildingByAssignedOfficer(saveInfos[i].Key,
                        out var building) == true)
                {
                    saveInfos.Remove(saveInfos[i]);
                    i--;
                }
            }

            foreach (var officerSaveInfo in saveInfos)
            {
                var info = _gameConfig.OfficersInfoConfig.GetItemByKey(officerSaveInfo.Key);
                var levelInfo = _gameConfig.OfficersLevelsInfoConfig.GetItemByKey(info.OfficerRarity, officerSaveInfo.Level);
                
                var fullInfo = new OfficerInfo();
                fullInfo.Info = info;
                fullInfo.LevelInfo = levelInfo;
                fullInfo.SaveInfo = officerSaveInfo;
                
                infos.Add(fullInfo);
            }
            
            View.NoOfficersTransform.gameObject.SetActive(saveInfos.Count == 0);
            
            infos = infos.OrderByDescending(o => o.LevelInfo.IncomeMultiplier).ToList();
            
            foreach (var officerSaveInfo in infos)
            {

                if (officerSaveInfo.Info.OfficerType == _building.Info.BuildingType)
                {
                    var view = View.OfficersChoicePool.GetObject();
                    view.Init(_spritesConfig, _gameConfig, _prefsSaveManager, _uiManager);
                    view.Show(SelectOfficer, officerSaveInfo, true, autoLevel);
                }
            }
            
        }

        private void GoToUnavailableOfficers()
        {
            View.SuitableButton.ChangeState();
            View.NotSuitableButton.ChangeState();
            
            View.OfficersChoicePool.ReturnAll();
            
            var saveInfos = _prefsSaveManager.PrefsData.OfficersModel.OfficerSaveInfos;
            var infos = new List<OfficerInfo>();
            
            View.NoOfficersTransform.gameObject.SetActive(saveInfos.Count == 0);
            
            foreach (var officerSaveInfo in saveInfos)
            {
                var info = _gameConfig.OfficersInfoConfig.GetItemByKey(officerSaveInfo.Key);
                var levelInfo = _gameConfig.OfficersLevelsInfoConfig.GetItemByKey(info.OfficerRarity, officerSaveInfo.Level);
                
                var fullInfo = new OfficerInfo();
                fullInfo.Info = info;
                fullInfo.LevelInfo = levelInfo;
                fullInfo.SaveInfo = officerSaveInfo;
                
                infos.Add(fullInfo);
            }
            
            View.NoOfficersTransform.gameObject.SetActive(saveInfos.Count == 0);
            
            infos = infos.OrderByDescending(o => o.LevelInfo.IncomeMultiplier).ToList();
            
            foreach (var officerSaveInfo in infos)
            {

                if (officerSaveInfo.Info.OfficerType != _building.Info.BuildingType)
                {
                    var view = View.OfficersChoicePool.GetObject();
                    view.Init(_spritesConfig, _gameConfig, _prefsSaveManager, _uiManager);
                    view.Show(SelectOfficer, officerSaveInfo, false, 0);
                }
            }
        }

        private void SelectOfficer(string officerKey)
        {
            _uiManager.HideLast(); 
            _action(officerKey, false);
        }
    }

    public struct OfficerInfo
    {
        public OfficerLevelInfoConfigInitializator LevelInfo;
        public OfficerConfigInitializator Info;
        public OfficerSaveInfo SaveInfo;
    }
}