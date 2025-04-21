using System.Collections.Generic;
using Gameplay.Scripts;
using Gameplay.Scripts.Agents;
using Gameplay.Scripts.Buildings;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.LockedZoneManagement;
using Gameplay.Scripts.Locker;
using Gameplay.Scripts.Missions;
using Gameplay.Scripts.Tutorial;
using Gameplay.Scripts.Utils;
using Managers;
using UI;
using UI.Scripts.DialogWindow;
using UI.Scripts.UpgradeBuildingWindow;
using UI.Scripts.UpgradeWindow;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Tutorial
{
    public class UpgradeTutorial : ITutorial, IInstall
    {
        private UIManager _uiManager;
        private PlayerPrefsSaveManager _playerPrefsSaveManager;
        private LockController _lockController;
        private TutorialManager _tutorialManager;
        private OpenDistrictLocker tutorialLocker;
        private SignalBus _signalBus;
        private UpgradeBaseLocker _upgradeBaseLocker;
        private TutorialTargetsContainer _targetsContainer;
        private OrderSpawnLocker orderSpawnLocker;
        private CameraController _cameraController;
        private LootMissionLocker _lootMissionLocker;
        
        private const float DelayBetweenCards = 0.4f;

        private const string BaseUpgrade = "BaseUpgrade";
        private const string UpgradeButton = "UpgradeButton";

        [Inject]
        private void Construct(UIManager uiManager,
             PlayerPrefsSaveManager userProfileManager,
             LockController lockController,
             TutorialManager tutorialManager,
             SignalBus signalBus,
             CameraController cameraController)
         {
             _cameraController = cameraController;
             _signalBus = signalBus;
             _uiManager = uiManager;
             _playerPrefsSaveManager = userProfileManager;
             _lockController = lockController;
             _tutorialManager = tutorialManager;
         }
        
        public void Configurate()
        {
            
        }

        public List<TutorialStepBase> GetSteps()
        {
            return new List<TutorialStepBase>()
            {
                new TutorialWaitForSeconds(0.5f),
                
                new TutorialSilentStepAction(async () =>
                {
                    _playerPrefsSaveManager.DisableSave();
                    _playerPrefsSaveManager.PrefsData.CurrenciesModel.ChangeMoney(5, 0);
                    _cameraController.EnabledLeanDragCamera(false);
                    _cameraController.EnabledLeanCameraZoom(false);
                }),
                
                new TutorialSilentStepAction(() =>
                {
                    _upgradeBaseLocker = _lockController.AddLock<UpgradeBaseLocker>();
                    _signalBus.Fire(new MainScreenTutorialStepSignal(MainScreenWidgetBlockType.UpgradesBlock));
                }),
                
                
                DialogWindowStep(DialogueName.tutorial_phrase_5),
                new TutorialWaiterForPointClick(),

                new TutorialSilentStepAction((() =>
                {
                    _uiManager.HideLast();
                })),
                
                
                new TutorialChain(new TutorialStepBase[]
                {
                    new TutorialFingerTapStep(target => target.HaveTag(UpgradeButton)).SetIncline().SetScale(0.5f),
                    new TutorialWaitForScreen<UpgradeWindow>(),
                }),
                
                new TutorialWaitForSeconds(0.25f),
                
                new TutorialChain(new TutorialStepBase[]
                {
                    new TutorialFingerTapStep(target => target.HaveTag(BaseUpgrade)).SetIncline().SetScale(0.5f),
                    new TutorialWaitForSignal<BaseUpgradeSignal>(),
                }),
                
                DialogWindowStep(DialogueName.tutorial_phrase_6),
                new TutorialWaiterForPointClick(),
                
                new TutorialSilentStepAction(() =>
                {
                    _uiManager.HideLast();
                    _playerPrefsSaveManager.DisableSave();
                    
                    _playerPrefsSaveManager.PrefsData.TutorialModel.SetTutorCompleted(TutorialType.UpgradeTutorial);
                    _lockController.ResetLockers();
                    _playerPrefsSaveManager.ForceSave();
                    
                    _cameraController.EnabledLeanDragCamera(true);
                    _cameraController.EnabledLeanCameraZoom(true);
                    
                    _signalBus.Fire(new MainScreenTutorialStepSignal(MainScreenWidgetBlockType.Other,true));
                }),
            };
        }

        public TutorialChain DialogWindowStep(DialogueName phase, bool withBackground = false)
        {
            return new TutorialChain(new TutorialStepBase[]
            {
                new TutorialSilentStepAction(() =>
                {
                    var args = new DialogWindowArguments(phase, true);
                    _uiManager.Show<DialogWindowController>(args);
                }),
                new TutorialWaitForSeconds(DelayBetweenCards),
            });
        }

        public void Install(TutorialServiceLocator tutorialServiceLocator)
        {
            _targetsContainer = tutorialServiceLocator.GetService<TutorialTargetsContainer>();
        }
    }
}