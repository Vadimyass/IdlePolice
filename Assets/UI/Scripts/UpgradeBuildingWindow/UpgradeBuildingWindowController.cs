using System;
using System.Collections.Generic;
using System.Linq;
using Audio;
using BigD.Config;
using Cysharp.Threading.Tasks;
using Gameplay.Configs;
using Gameplay.Scripts;
using Gameplay.Scripts.Buildings;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.LevelManagement;
using Gameplay.Scripts.Utils;
using MyBox;
using SolidUtilities;
using Tutorial;
using UI.Scripts.OfficerChoiceWidget;
using UI.Scripts.OfficerInfoWidget;
using UI.UIUtils;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Scripts.UpgradeBuildingWindow
{
    public class UpgradeBuildingWindowController : UIScreenController<UpgradeBuildingWindow>
    {
        private Building _building;
        private bool _singleUpgradeMode;
        private GameConfig _gameConfig;
        private UIManager _uiManager;
        private PlayerPrefsSaveManager _prefsSaveManager;
        private SpritesConfig _spritesConfig;
        private CameraController _cameraController;
        private double _cost;
        private LevelController _levelController;
        private IReadOnlyList<Building> _buildingsInParent;
        private SignalBus _signalBus;

        [Inject]
        private void Construct(GameConfig gameConfig, SignalBus signalBus, CameraController cameraController, LevelController levelController, PlayerPrefsSaveManager prefsSaveManager, SpritesConfig spritesConfig, UIManager uiManager)
        {
            _signalBus = signalBus;
            _levelController = levelController;
            _cameraController = cameraController;
            _spritesConfig = spritesConfig;
            _prefsSaveManager = prefsSaveManager;
            _uiManager = uiManager;
            _gameConfig = gameConfig;
        }

        public override void Init(UIScreen uiScreen)
        {
            base.Init(uiScreen);
            View.OfficerCard.Init(_spritesConfig, _prefsSaveManager, _uiManager);
            View.OfficerCard.gameObject.SetActive(false);
        }

        public override async UniTask Display(UIArguments arguments)
        {
            await base.Display(arguments);
            _building = ((UpgradeBuildingWindowArguments)arguments).Building;
            _singleUpgradeMode = false;

            if (_prefsSaveManager.PrefsData.TutorialModel.IsContextTutorDone(TutorialType.WelcomeTutorial) == false)
            {
                _singleUpgradeMode = true;
            }
            
            ChangeUpgradeMode();
            
            View.NextBuildingButton.onClick.RemoveAllListeners();
            View.PreviousBuildingButton.onClick.RemoveAllListeners();
            View.UpgradeButton.onClick.RemoveAllListeners();
            View.NotEnoughButton.onClick.RemoveAllListeners();
            View.SwitchUpgradeButton.onClick.RemoveAllListeners();
            View.ChoseCapoButton.onClick.RemoveAllListeners();
            
            View.CloseButton.onClick.AddListener(() =>
            {
                _audioManager.PlaySound(TrackName.ButtonClick);
                _uiManager.HideLast();
            });
            View.CloseButton2.onClick.AddListener(() =>
            {
                _audioManager.PlaySound(TrackName.ButtonClick);
                _uiManager.HideLast();
            });
            
            View.NextBuildingButton.onClick.AddListener(ToNextBuilding);
            View.PreviousBuildingButton.onClick.AddListener(ToPreviousBuilding);
            
            _buildingsInParent = _levelController.CurrentLevel.GetAllBuiltBuildings().ToList();

            View.NextBuildingButton.gameObject.SetActive(_buildingsInParent.Count > 1);
            View.PreviousBuildingButton.gameObject.SetActive(_buildingsInParent.Count > 1);
            
            View.SwitchUpgradeButton.onClick.AddListener(ChangeUpgradeMode);
            View.UpgradeButton.onClick.AddListener(Upgrade);
            View.NotEnoughButton.onClick.AddListener(Upgrade);
            View.ChoseCapoButton.onClick.AddListener(GoToChoiceOfficer);

            SetBuildingInfo();
        }

        private void SetBuildingInfo()
        {
            var info = _gameConfig.UpgradeConfig.GetBuildingByKey(_building.BuildingKey, _levelController.CurrentLevel.Level);
            View.NameText.text = info.Name;

            foreach (var typeSprite in View.TypeSprites)
            {
                typeSprite.Value.gameObject.SetActive(false);
            }
            
            View.TypeSprites[_building.Info.BuildingType].gameObject.SetActive(true);
            
            foreach (var typeSprite in View.TypeSpritesOfficer)
            {
                typeSprite.Value.gameObject.SetActive(false);
            }
            
            View.TypeSpritesOfficer[_building.Info.BuildingType].gameObject.SetActive(true);

            SetOfficer(_building.OfficerSet, true);
            
            RefreshInfo();
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(View.NameHorizontalGroup);
        }

        private void RefreshInfo()
        {


            var currentLevel = _gameConfig.UpgradeConfig.GetItemByKey(_building.BuildingKey, _building.Level,_levelController.CurrentLevel.Level);
            var nextStage = _gameConfig.UpgradeConfig
                .GetNextStageByLevel(_building.BuildingKey, _building.Level, _levelController.CurrentLevel.Level);
            var previousStage = _gameConfig.UpgradeConfig
                .GetPreviousStageByLevel(_building.BuildingKey, _building.Level, _levelController.CurrentLevel.Level);
            View.IncomeText.text = TextMeshProUtils.NumberToShortenedText(_building.RealIncome);
            View.LevelText.text = "Lv." + currentLevel.Level;
            
            View.CarsText.text = currentLevel.Cars.ToString();
            
            View.AutomaticText.text = _building.IsAutomated == true ? "ON" : "OFF"; 
            View.AutomaticText.color = _building.IsAutomated == true ? Color.green : Color.red; 
            
            View.UpgradeButton.gameObject.SetActive(true);
            
            View.MultiplierText.transform.parent.gameObject.SetActive(true);
            
            View.CarsText.transform.parent.gameObject.SetActive(currentLevel.Cars > 0);

            View.ItemImage.sprite = _spritesConfig.GetSpriteByName(currentLevel.SpriteName);
            View.ItemsNameText.text = currentLevel.UpgradeName;
            
            if (nextStage == null)
            {
                if (previousStage.Multiplier == 1)
                {
                    View.MultiplierText.transform.parent.gameObject.SetActive(false);
                }

                foreach (var transform in View.HideOnMaxLevel)
                {
                    transform.gameObject.SetActive(false);
                }
                
                foreach (var transform in View.ShowOnMaxLevel)
                {
                    transform.gameObject.SetActive(true);
                }
                
                View.MultiplierText.text = "x" + previousStage.Multiplier;
                View.ProgressSlider.minValue = 0;
                View.ProgressSlider.maxValue = 1;
                View.ProgressSlider.value = 1;
                View.DurationText.text = _building.RealDuration.ToString("0.00");
                View.UpgradeButton.gameObject.SetActive(false);
                return;
            }
            else
            {
                foreach (var transform in View.HideOnMaxLevel)
                {
                    transform.gameObject.SetActive(true);
                }
                
                foreach (var transform in View.ShowOnMaxLevel)
                {
                    transform.gameObject.SetActive(false);
                }

            }
            
            if (nextStage.Multiplier == 1)
            {
                View.MultiplierText.transform.parent.gameObject.SetActive(false);
            }
            View.MultiplierText.text = "x" + nextStage.Multiplier;
            
            View.ProgressSlider.minValue = previousStage.Level;
            View.ProgressSlider.maxValue = nextStage.Level;
            
            View.FutureProgressSlider.minValue = previousStage.Level;
            View.FutureProgressSlider.maxValue = nextStage.Level;
            
            View.ProgressSlider.value = currentLevel.Level;

            View.DurationText.text = _building.RealDuration.ToString("0.00");
            RefreshUpgradeInfo();
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(View.LevelHorizontalGroup);
        }

        private void RefreshUpgradeInfo()
        {
            var currentLevel = _gameConfig.UpgradeConfig.GetItemByKey(_building.BuildingKey, _building.Level, _levelController.CurrentLevel.Level);
            var nextLevel = new UpgradeConfigInitializator();

            if (_singleUpgradeMode)
            {
                nextLevel = _gameConfig.UpgradeConfig.GetItemByKey(_building.BuildingKey, _building.Level+1, _levelController.CurrentLevel.Level);
            }
            else
            {
                nextLevel = _gameConfig.UpgradeConfig.GetNextStageByLevel(_building.BuildingKey, _building.Level, _levelController.CurrentLevel.Level);
            }

            if(nextLevel == null) return;
            
            View.FutureLevelsText.text = "+" + (nextLevel.Level - currentLevel.Level).ToString();
            View.FutureIncomeText.text = TextMeshProUtils.NumberToShortenedText(nextLevel.Income * _building.GetFullBuildingModifiers().IncomeMultiplier);
            _cost = 0;

            for (int i = currentLevel.Level; i <= nextLevel.Level; i++)
            {
                _cost += _gameConfig.UpgradeConfig.GetItemByKey(_building.BuildingKey, i, _levelController.CurrentLevel.Level).UpgradeCost;
            }

            var isEnoughMoney = _prefsSaveManager.PrefsData.CurrenciesModel.BasesMoney[_levelController.CurrentLevel.Level] >= _cost;
            
            View.CarsAddText.gameObject.SetActive(nextLevel.Cars - currentLevel.Cars > 0);
            View.CarsAddText.text = "+" + (nextLevel.Cars - currentLevel.Cars); 
            
            View.UpgradeButton.gameObject.SetActive(isEnoughMoney);
            View.NotEnoughButton.gameObject.SetActive(!isEnoughMoney);
            
            View.CostText2.text = TextMeshProUtils.NumberToShortenedText(_cost);
            View.CostText.text = TextMeshProUtils.NumberToShortenedText(_cost);
            View.FutureProgressSlider.value = nextLevel.Level;
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(View.HorizontalGroup);
            
        }
        
        private void GoToChoiceOfficer()
        {
            _audioManager.PlaySound(TrackName.ButtonClick);
            _uiManager.Show<OfficerChoiceWidgetController>(new OfficerChoiceWidgetArguments(_building, SetOfficer));
        }

        private void SetOfficer(string officer, bool withSave = false)
        {
            View.OfficerCard.gameObject.SetActive(true);
            var info = _gameConfig.OfficersInfoConfig.GetItemByKey(officer);
            if (officer != String.Empty)
            {
                var save = _prefsSaveManager.PrefsData.OfficersModel.GetSaveInfo(officer);
                var fullInfo = new OfficerInfo();
                fullInfo.Info = info;
                fullInfo.SaveInfo = save;
                
                View.OfficerCard.Show(fullInfo);
            }
            else
            {
                View.OfficerCard.gameObject.SetActive(false);
            }

            _building.SetOfficer(officer, withSave);
            RefreshInfo();
        }

        private void Upgrade()
        {
            bool milestoneComplete;

            if (_prefsSaveManager.PrefsData.CurrenciesModel.TrySpendMoney(_cost, _levelController.CurrentLevel.Level) ==
                false)
            {
                _signalBus.Fire(new NotEnoughCurrencySignal(CurrencyUIType.Dollar));
                return;
            }
            
            if (_singleUpgradeMode == true)
            {
                milestoneComplete = _gameConfig.UpgradeConfig
                    .GetItemByKey(_building.BuildingKey, _building.Level + 1, 0).Multiplier != 0;
            }
            else
            {
                milestoneComplete = true;
            }

            _audioManager.PlaySound(TrackName.UpgradeButton);
            _building.Upgrade(milestoneComplete);
            RefreshInfo();
        }
        
        private void ChangeUpgradeMode()
        {
            _audioManager.PlaySound(TrackName.ButtonClick);
            _singleUpgradeMode = !_singleUpgradeMode;
            View.MaxText.color = _singleUpgradeMode ? Color.gray : Color.white;
            View.X1Text.color = !_singleUpgradeMode ? Color.gray : Color.white;
            View.SingleModeImage.enabled = _singleUpgradeMode;
            View.MaxModeImage.enabled = !_singleUpgradeMode;
            RefreshUpgradeInfo();
        }
        
        private void ToNextBuilding()
        {

            _audioManager.PlaySound(TrackName.Change_Building_Sound);
            
            var index = _buildingsInParent.IndexOf(_building);
            do
            {
                if (index + 1 >= _buildingsInParent.Count)
                {
                    index = 0;
                }
                else
                {
                    index++;
                }
            } while (_buildingsInParent[index].IsBuilt == false);
            
            _building = _buildingsInParent[index];
            
            _cameraController.FocusOnBuilding(_building.transform, true);
            
            SetBuildingInfo();
        }

        private void ToPreviousBuilding()
        {

            _audioManager.PlaySound(TrackName.Change_Building_Sound);
            
            var index = _buildingsInParent.IndexOf(_building);
            do
            {
                if (index - 1 < 0)
                {
                    index = _buildingsInParent.Count - 1;
                }
                else
                {
                    index--;
                } 
            } while (_buildingsInParent[index].IsBuilt == false);

            _building = _buildingsInParent[index];
            
            _cameraController.FocusOnBuilding(_building.transform, true);
            
            SetBuildingInfo();
        }
        
        
        public override UniTask OnHide()
        {
            _building.SaveData();
            _cameraController.ComeBackFieldOfView();
            return base.OnHide();
        }
    }
}