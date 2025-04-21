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
using UI.Scripts.TravelWindow;
using UI.Scripts.UpgradeBuildingWindow;
using UI.Scripts.UpgradeWindow;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Tutorial
{
    public class NewBaseTutorial : ITutorial, IInstall
    {
        private UIManager _uiManager;
        private PlayerPrefsSaveManager _playerPrefsSaveManager;
        private LockController _lockController;
        private SignalBus _signalBus;
        private UpgradeBaseLocker _upgradeBaseLocker;
        private CameraController _cameraController;
        private ShopLocker _shopLocker;
        private TutorialTargetsContainer _targetsContainer;

        private const float DelayBetweenCards = 0.4f;

        private const string NextBaseButton = "NextBaseButton";
        
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
                }),
                
                DialogWindowStep(DialogueName.tutorial_phrase_12),
                new TutorialWaiterForPointClick(),
                
                new TutorialSilentStepAction(() =>
                {
                    _uiManager.HideLast();
                }),
                
                new TutorialChain(new TutorialStepBase[]
                {
                    new TutorialFingerTapStep(target => target.HaveTag(NextBaseButton)).SetIncline().SetScale(0.5f),
                    new TutorialWaitForScreen<TravelWindow>(),
                }),
                
                new TutorialSilentStepAction(() =>
                {
                    _cameraController.EnabledLeanDragCamera(true);
                    _cameraController.EnabledLeanCameraZoom(true);
                    _playerPrefsSaveManager.DisableSave();
                    
                    _playerPrefsSaveManager.PrefsData.TutorialModel.SetTutorCompleted(TutorialType.NewBaseTutorial);
                    _lockController.ResetLockers();
                    _playerPrefsSaveManager.ForceSave();
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