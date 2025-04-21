
using Managers;
using Signals;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay.Scripts.Tutorial;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Tutorial
{
    public class TestTutorial : ITutorial, IInstall
    {
        public static string TagBuildingButton => "ConstructionButton";
        public static string TagBuyButton => "BuyButton";
        public static string TagCrown => "Crown";
        public static string TagMainHud => "HUD";
        public static string TagNameContainer => "NameContainer";
        public static string TagRecoveryButton => "RecoveryButton";
        public static string TagSwipeTarget => "SwipeTarget";
        public static string TagMenuButton => "MenuButton";
        public static string TagCloseButton => "CloseButton";
        public static string TagCloseBG => "CloseBG";


        private const string BuilderMessage = "tutorial.unlockcastles";
        private const string ProfessorMessage = "tutorial.outofcoins";

        private const int SortingOrder = 301;
        
        // private UIManager _uiManager;
        // private UserProfileManager _userProfileManager;
        // private LockController _lockController;
        // private IUiWindowFilter _windowFilter;
        // private TutorialManager _tutorialManager;
        //
        // private TutorialTargetsContainer _targetsContainer;
        //
        // private Button _closeButton;
        // private Button _closeBG;
        //
        // [Inject]
        // private void Construct(UIManager uiManager,
        //     UserProfileManager userProfileManager,
        //     LockController lockController,
        //     TutorialManager tutorialManager)
        // {
        //     _uiManager = uiManager;
        //     _userProfileManager = userProfileManager;
        //     _lockController = lockController;
        //     _tutorialManager = tutorialManager;
        // }
        //
        // public void Configurate()
        // {
        //     _windowFilter = new UiWindowFilterAvailableWindows(
        //         typeof(WelcomeTutorialPanelController));
        // }
        //
        // public List<TutorialStepBase> GetSteps()
        // {
        //     var changePageLocker = _lockController.AddLock<ChangePageLocker>();
        //     var menuButton = _targetsContainer.GetTarget(target => target.HaveTag(TagMenuButton));
        //
        //     CanvasGroup containerHud = null;
        //     CanvasGroup containerNameBox = null;
        //     CanvasGroup containerRecoveryButton = null;
        //     Button buildingButton = null;
        //
        //     var isConstructionButtonClicked = false;
        //
        //     return new List<TutorialStepBase>()
        //     {
        //         new TutorialSilentStepAction(() =>
        //         {
        //             _tutorialManager.SendTutorialStepEvent(
        //                 TutorialType.WelcomeTutorial.ToString(), 
        //                 AnalyticsConstants.ParametersValues.Start);
        //             
        //             menuButton.gameObject.SetActive(false);
        //         }),
        //
        //         new TutorialConditionWaiter(() => !_uiManager.IsScreenOpened<PlayerProfilePanelController>()),
        //         new TutorialConditionWaiter(() => _uiManager.IsScreenOpened<GameModeUserScreenController>()),
        //
        //         new TutorialSilentStepAction(() =>
        //         {
        //             containerHud = _targetsContainer.GetTarget(target => target.HaveTag(TagMainHud)).GetComponent<CanvasGroup>();
        //             containerNameBox = _targetsContainer.GetTarget(target => target.HaveTag(TagNameContainer)).GetComponent<CanvasGroup>();
        //             containerRecoveryButton = _targetsContainer.GetTarget(target => target.HaveTag(TagRecoveryButton)).GetComponent<CanvasGroup>();
        //             menuButton = _targetsContainer.GetTarget(target => target.HaveTag(TagMenuButton));
        //             buildingButton = _targetsContainer.GetTarget(target => target.HaveTag(TagBuildingButton)).GetComponent<Button>();
        //             
        //             _userProfileManager.IsSaveLocked = true;
        //
        //             _uiManager.UiFilter.AddCreateFilter(_windowFilter);
        //             _uiManager.Show<WelcomeTutorialPanelController>();
        //             
        //             var gameModeUserScreen = (GameModeUserScreen) _uiManager.GetScreen<GameModeUserScreenController>().UIScreen;
        //             gameModeUserScreen.CanvasGroup.alpha = 1;
        //             
        //             containerHud.alpha = 0;
        //             containerNameBox.alpha = 0;
        //             containerRecoveryButton.alpha = 0;
        //             containerRecoveryButton.interactable = false;
        //
        //             buildingButton.onClick.AddListener(() => isConstructionButtonClicked = true);
        //         }),
        //
        //
        //         new TutorialChain(new TutorialStepBase[]
        //         {
        //             new TutorialConditionWaiter(() => isConstructionButtonClicked),
        //             new TutorialFingerTapStep(target => target.HaveTag(TagBuildingButton)).SetIncline().SetScale(0.7f),
        //         }),
        //
        //         new TutorialSilentStepAction(() =>
        //         {
        //             _uiManager.Hide<WelcomeTutorialPanelController>();
        //             buildingButton.onClick.RemoveListener(() => isConstructionButtonClicked = true);
        //         }),
        //
        //         new TutorialWaitForScreenClosed<WelcomeTutorialPanel>(),
        //         new TutorialSilentStepAction(() =>
        //         {
        //             _uiManager.UiFilter.RemoveCreateFilter(_windowFilter);
        //             _uiManager.Show<LocationShopPanelController>(new LocationShopPanelArg(TutorialType.WelcomeTutorial));
        //         }),
        //         new TutorialWaitForScreen<LocationShopPanel>(),
        //
        //         new TutorialSilentStepAction(() =>
        //         {
        //             containerHud.alpha = 1;
        //
        //             _closeButton = _targetsContainer.GetTarget(target => target.HaveTag(TagCloseButton)).GetComponent<Button>();
        //             _closeBG = _targetsContainer.GetTarget(target => target.HaveTag(TagCloseBG)).GetComponent<Button>();
        //             _closeButton.interactable = false;
        //             _closeBG.interactable = false;
        //         }),
        //
        //         new TutorialChain(new TutorialStepBase[]
        //         {
        //             new TutorialWaitForScreenClosed<LocationShopPanel>().SetWaitStartScreenHide(),
        //             new TutorialFingerTapStep(target => target.HaveTag(TagBuyButton)).SetAngle(0).SetScale(0.7f),
        //         }),
        //
        //         new TutorialSilentStepAction(() =>
        //         {
        //             _closeButton.interactable = true;
        //             _closeBG.interactable = true;
        //
        //             buildingButton.enabled = false;
        //         }),
        //
        //         new TutorialWaitForSignal<BuildingConstructionStateSignal>(obj => obj.State == BuildingConstructionStateSignal.ConstructionState.Completed),
        //
        //         new TutorialSilentStepAction(() =>
        //         {
        //             var canvas = containerHud.GetComponent<Canvas>();
        //             canvas.overrideSorting = true;
        //             canvas.sortingOrder = SortingOrder;
        //         }),
        //         
        //         new TutorialChain(new TutorialStepBase[]
        //         {
        //             new TutorialDialogStep(BuilderMessage, AssistantType.Builder).SetPosition(new Vector2(0f, -200f)),
        //             new TutorialArrowPointerStep(target => target.HaveTag(TagCrown)),
        //         }),
        //
        //         new TutorialWaitForSeconds(0.2f),                
        //
        //         new TutorialSilentStepAction(() => 
        //         {
        //             var canvas = containerHud.GetComponent<Canvas>();
        //             canvas.overrideSorting = false;
        //             changePageLocker.Dispose();
        //             containerNameBox.alpha = 1;
        //         }),
        //
        //         new TutorialChain(new TutorialStepBase[]
        //         {
        //             new TutorialDialogStep(ProfessorMessage, AssistantType.Professor).SetButtonAvailable(false).SetAvailableToInputSignal(),
        //             new TutorialFingerTapStep(target =>  target.HaveTag(TagSwipeTarget), TutorialFingerTapStep.TapType.Swipe).SetScale(0.7f).SetOffset(Vector2.down * 200f).SetWaitCondition(
        //                 async () =>
        //                 {
        //                     await UniTask.WaitUntil(() => _uiManager.IsScreenOpened<DialogTutorialPanelController>());
        //                 }),
        //             new TutorialWaitForSignal<InputGestureSignal>(signal => signal.GestureType == InputController.GestureType.SwipeDown),
        //         }),
        //
        //         new TutorialSilentStepAction(() =>
        //         {
        //             _tutorialManager.SendTutorialStepEvent(
        //                 TutorialType.WelcomeTutorial.ToString(), 
        //                 AnalyticsConstants.ParametersValues.Finish);
        //
        //             _userProfileManager.IsSaveLocked = false;
        //
        //             _userProfileManager.UserProfileData.TutorialModel.SetTutorCompleted(TutorialType.WelcomeTutorial);
        //             _userProfileManager.ForceSaveUserProfile();
        //             menuButton.gameObject.SetActive(true);
        //             buildingButton.GetComponent<Button>().enabled = true;
        //             containerRecoveryButton.alpha = 1;
        //             containerRecoveryButton.interactable = true;
        //         })
        //     };
        // }
        //
        // public void Install(TutorialServiceLocator tutorialServiceLocator)
        // {
        //     _targetsContainer = tutorialServiceLocator.GetService<TutorialTargetsContainer>();
        // }
        public void Configurate()
        {
            throw new System.NotImplementedException();
        }

        public List<TutorialStepBase> GetSteps()
        {
            throw new System.NotImplementedException();
        }

        public void Install(TutorialServiceLocator tutorialServiceLocator)
        {
            throw new System.NotImplementedException();
        }
    }
}
