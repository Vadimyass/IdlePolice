using Gameplay.Scripts.Agents;
using Gameplay.Scripts.Buildings;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.GameBootstrap;
using Gameplay.Scripts.LockedZoneManagement;
using Gameplay.Scripts.Missions;
using Gameplay.Scripts.TimeManagement;
using Gameplay.Scripts.Tutorial;
using UI.Scripts.MainScreen;
using UI.Scripts.ShopWindow;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Scripts.Signals
{
    public class SignalsDeclarator : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<UpgradeBuildingSignal>();
            Container.DeclareSignal<BuildBuildingSignal>();
            Container.DeclareSignal<MissionCompleteSignal>();
            Container.DeclareSignal<MissionCheckSignal>();
            Container.DeclareSignal<ProcessFinishSignal>();
            Container.DeclareSignal<OpenDistrictZoneSignal>();
            Container.DeclareSignal<ChangeCurrencySignal>();
            Container.DeclareSignal<UIScreenChangeStateSignal>();
            Container.DeclareSignal<NotEnoughCurrencySignal>();
            Container.DeclareSignal<TimeTickSignal>();
            Container.DeclareSignal<BaseUpgradeSignal>();
            Container.DeclareSignal<ZoneUnlockSignal>();
            Container.DeclareSignal<RefreshShopSignal>();
            Container.DeclareSignal<LoadingProgressSignal>();
            Container.DeclareSignal<MainScreenTutorialStepSignal>();
            Container.DeclareSignal<CreateCarSignal>();
            Container.DeclareSignal<TutorialTouch>();
            Container.DeclareSignal<OfficerGetSignal>();
            Container.DeclareSignal<MilestoneCompleteSignal>();
            Container.DeclareSignal<GachaBoxGetSignal>();
        }
    }
}