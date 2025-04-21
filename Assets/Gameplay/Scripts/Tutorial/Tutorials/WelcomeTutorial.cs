using System.Collections.Generic;
using Gameplay.Scripts;
using Gameplay.Scripts.Agents;
using Gameplay.Scripts.Buildings;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.DataProfiling.PrefsData;
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
    public class WelcomeTutorial : ITutorial, IInstall
    {
        private UIManager _uiManager;
        private PlayerPrefsSaveManager _playerPrefsSaveManager;
        private LockController _lockController;
        private TutorialManager _tutorialManager;
        private OrderTickLocker tutorialLocker;
        private SignalBus _signalBus;
        private UpgradeBaseLocker _upgradeBaseLocker;
        private TutorialTargetsContainer _targetsContainer;
        private OrderSpawnLocker orderSpawnLocker;
        private CameraController _cameraController;
        private LootMissionLocker _lootMissionLocker;
        
        private const float DelayBetweenCards = 0.4f;


        private const string PoliceGarage = "PoliceGarage";
        private const string UpgradeBuildingButton = "UpgradeBuildingButton";
        private const string ChangeUpgradeMode = "ChangeUpgradeMode";
        private const string CarBox = "CarBox";
        private const string MoneyHud = "MoneyHud";
        private const string MissionView = "MissionView";

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
                new TutorialWaitForSeconds(1),
                
                new TutorialSilentStepAction(async () =>
                {
                    _playerPrefsSaveManager.DisableSave();
                    tutorialLocker = _lockController.AddLock<OrderTickLocker>();
                    orderSpawnLocker = _lockController.AddLock<OrderSpawnLocker>();
                    _cameraController.EnabledLeanDragCamera(false);
                    _cameraController.EnabledLeanCameraZoom(false);
                }),
                
                DialogWindowStep(DialogueName.tutorial_phrase_1),
                new TutorialWaiterForPointClick(),
                
                new TutorialSilentStepAction((() =>
                {
                    _uiManager.HideLast();
                })),
                
                new TutorialChain(new TutorialStepBase[]
                {
                    new TutorialFingerTapStep(target => target.HaveTag(CarBox)).SetIncline().SetScale(0.5f),
                    new TutorialWaitForSignal<CreateCarSignal>(),
                }),
                
                new TutorialSilentStepAction((() =>
                {
                    _uiManager.HideLast();
                    _lockController.RemoveLock(tutorialLocker);
                })),
                
                new TutorialWaitForSignal<ProcessFinishSignal>(x => x.BuildingProcessName == BuildingProcessName.Arrest),
                
                DialogWindowStep(DialogueName.tutorial_phrase_2),
                new TutorialWaiterForPointClick(),
                
                new TutorialSilentStepAction((() =>
                {
                    _uiManager.HideLast();
                })),
                
                new TutorialWaitForSeconds(0.25f),
                
                new TutorialChain(new TutorialStepBase[]
                {
                    new TutorialFingerTapStep(target => target.HaveTag(MoneyHud)).SetIncline().SetScale(0.5f),
                    new TutorialWaitForSignal<ChangeCurrencySignal>(),
                }),
                
                new TutorialSilentStepAction(() =>
                {
                    _uiManager.HideLast();
                    _lockController.RemoveLock(orderSpawnLocker);
                    _lootMissionLocker = _lockController.AddLock<LootMissionLocker>();
                }),
                
                DialogWindowStep(DialogueName.tutorial_phrase_3),
                new TutorialWaiterForPointClick(),
                
                new TutorialSilentStepAction(() =>
                {
                    _uiManager.HideLast();
                }),
                
                new TutorialChain(new TutorialStepBase[]
                {
                    new TutorialFingerTapStep(target => target.HaveTag(PoliceGarage)).SetIncline().SetScale(0.5f),
                    new TutorialWaitForScreen<UpgradeBuildingWindow>(),
                }),
                
                new TutorialChain(new TutorialStepBase[]
                {
                    new TutorialFingerTapStep(target => target.HaveTag(UpgradeBuildingButton)).SetIncline().SetScale(0.5f),
                    new TutorialWaitForSignal<UpgradeBuildingSignal>(),
                }),
                
                new TutorialSilentStepAction(() =>
                {
                    _uiManager.HideLast();
                    _signalBus.Fire(new MainScreenTutorialStepSignal(MainScreenWidgetBlockType.MissionBlock));
                }),
                
                DialogWindowStep(DialogueName.tutorial_phrase_4),
                new TutorialWaiterForPointClick(),
                
                new TutorialSilentStepAction(() =>
                {
                    _uiManager.HideLast();
                }),
                
                new TutorialChain(new TutorialStepBase[]
                {
                    new TutorialFingerTapStep(target => target.HaveTag(MissionView)).SetIncline().SetScale(0.5f),
                    new TutorialWaitForSignal<ChangeCurrencySignal>(),
                }),
                
                
                new TutorialSilentStepAction(() =>
                {
                    _cameraController.EnabledLeanDragCamera(true);
                    _cameraController.EnabledLeanCameraZoom(true);
                    _playerPrefsSaveManager.DisableSave();
                    
                    _playerPrefsSaveManager.PrefsData.TutorialModel.SetTutorCompleted(TutorialType.WelcomeTutorial);
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