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
using UI.Scripts.GachaBoxOpenWindow;
using UI.Scripts.MainScreen;
using UI.Scripts.OfficerChoiceWidget;
using UI.Scripts.OfficersWindow;
using UI.Scripts.ShopWindow;
using UI.Scripts.UpgradeBuildingWindow;
using UI.Scripts.UpgradeWindow;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Tutorial
{
    public class GachaTutorial : ITutorial, IInstall
    {
        private UIManager _uiManager;
        private PlayerPrefsSaveManager _playerPrefsSaveManager;
        private LockController _lockController;
        private SignalBus _signalBus;
        private UpgradeBaseLocker _upgradeBaseLocker;
        private CameraController _cameraController;
        private ShopLocker _shopLocker;
        private TutorialTargetsContainer _targetsContainer;
        private BuildingLocker _locker;

        private const float DelayBetweenCards = 0.4f;

        private const string ShopButton = "ShopButton";
        private const string OfficerButton = "OfficerButton";
        private const string OpenBox = "OpenBox";
        private const string PoliceGarage = "PoliceGarage";
        private const string OfficerCard = "OfficerCard";
        private const string AssignOfficer = "AssignOfficer";
        
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
                    _cameraController.EnabledLeanDragCamera(false);
                    _cameraController.EnabledLeanCameraZoom(false);
                    _shopLocker = _lockController.AddLock<ShopLocker>();
                    _signalBus.Fire(new MainScreenTutorialStepSignal(MainScreenWidgetBlockType.ShopBlock));
                }),
                
                DialogWindowStep(DialogueName.tutorial_phrase_9),
                new TutorialWaiterForPointClick(),
                
                new TutorialSilentStepAction(() =>
                {
                    _uiManager.HideLast();
                }),
                
                new TutorialChain(new TutorialStepBase[]
                {
                    new TutorialFingerTapStep(target => target.HaveTag(ShopButton)).SetIncline().SetScale(0.5f),
                    new TutorialWaitForScreen<ShopWindow>(),
                }),
                
                new TutorialWaitForSeconds(0.25f),
                
                new TutorialChain(new TutorialStepBase[]
                {
                    new TutorialFingerTapStep(target => target.HaveTag(OpenBox)).SetIncline().SetScale(0.5f),
                    new TutorialWaitForScreen<GachaBoxOpenWindow>(),
                }),
                
                new TutorialWaitForScreen<MainScreen>(),
                
                DialogWindowStep(DialogueName.tutorial_phrase_10),
                new TutorialWaiterForPointClick(),
                
                new TutorialSilentStepAction((() =>
                {
                    _uiManager.HideLast();
                })),
                
                new TutorialSilentStepAction((() =>
                {
                    var button = _targetsContainer.GetTarget((target => target.HaveTag(PoliceGarage))).transform;
                    _cameraController.FocusOnPosition(button.position);
                    _lockController.RemoveLock(_shopLocker);
                    _locker = _lockController.AddLock<BuildingLocker>();
                })),
                
                new TutorialChain(new TutorialStepBase[]
                {
                    new TutorialFingerTapStep(target => target.HaveTag(PoliceGarage)).SetIncline().SetScale(0.5f),
                    new TutorialWaitForScreen<UpgradeBuildingWindow>(),
                }),
                
                new TutorialWaitForSeconds(0.25f),
                
                new TutorialChain(new TutorialStepBase[]
                {
                    new TutorialFingerTapStep(target => target.HaveTag(OfficerCard)).SetIncline().SetScale(0.5f),
                    new TutorialWaitForScreen<OfficerChoiceWidget>(),
                }),
                
                new TutorialWaitForSeconds(0.25f),

                new TutorialChain(new TutorialStepBase[]
                {
                    new TutorialFingerTapStep(target => target.HaveTag(AssignOfficer)).SetIncline().SetScale(0.5f),
                    new TutorialWaitForScreen<UpgradeBuildingWindow>(),
                }),
                
                new TutorialSilentStepAction((() =>
                {
                    _uiManager.HideLast();
                    _signalBus.Fire(new MainScreenTutorialStepSignal(MainScreenWidgetBlockType.OfficersBlock));
                })),

                DialogWindowStep(DialogueName.tutorial_phrase_11),
                new TutorialWaiterForPointClick(),
                
                new TutorialSilentStepAction((() =>
                {
                    _uiManager.HideLast();
                })),


                new TutorialChain(new TutorialStepBase[]
                {
                    new TutorialFingerTapStep(target => target.HaveTag(OfficerButton)).SetIncline().SetScale(0.5f),
                    new TutorialWaitForScreen<OfficersWindow>(),
                }),

                
                new TutorialSilentStepAction(() =>
                {
                    _cameraController.EnabledLeanDragCamera(true);
                    _cameraController.EnabledLeanCameraZoom(true);
                    _playerPrefsSaveManager.DisableSave();
                    
                    _playerPrefsSaveManager.PrefsData.TutorialModel.SetTutorCompleted(TutorialType.ShopTutorial);
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