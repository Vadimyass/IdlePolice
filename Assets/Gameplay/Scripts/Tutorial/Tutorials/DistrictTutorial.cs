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
using UI.Scripts.OfficersWindow;
using UI.Scripts.UpgradeBuildingWindow;
using UI.Scripts.UpgradeWindow;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Tutorial
{
    public class DistrictTutorial : ITutorial, IInstall
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

        private const string OpenDistrictButton = "OpenDistrictButton";

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
                    tutorialLocker = _lockController.AddLock<OpenDistrictLocker>();
                    _cameraController.EnabledLeanDragCamera(false);
                    _cameraController.EnabledLeanCameraZoom(false);
                }),
                
                DialogWindowStep(DialogueName.tutorial_phrase_7),
                new TutorialWaiterForPointClick(),
                
                new TutorialSilentStepAction((() =>
                {
                    _uiManager.HideLast();
                    var button = _targetsContainer.GetTarget((target => target.HaveTag(OpenDistrictButton))).transform;
                    _cameraController.FocusOnPosition(button.position);
                })),
                
                new TutorialChain(new TutorialStepBase[]
                {
                    new TutorialFingerTapStep(target => target.HaveTag(OpenDistrictButton)).SetIncline().SetScale(0.5f),
                    new TutorialWaitForSignal<ZoneUnlockSignal>(),
                }),
                
                DialogWindowStep(DialogueName.tutorial_phrase_8),
                new TutorialWaiterForPointClick(),
                
                new TutorialSilentStepAction(() =>
                {
                    _uiManager.HideLast();
                    _playerPrefsSaveManager.DisableSave();
                    
                    _cameraController.EnabledLeanDragCamera(true);
                    _cameraController.EnabledLeanCameraZoom(true);
                    
                    _playerPrefsSaveManager.PrefsData.TutorialModel.SetTutorCompleted(TutorialType.OpenDistrict);
                    _lockController.ResetLockers();
                    _playerPrefsSaveManager.ForceSave();
                    
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