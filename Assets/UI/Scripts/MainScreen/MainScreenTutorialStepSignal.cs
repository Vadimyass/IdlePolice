using UI.Scripts.MainScreen;

namespace UnityEngine.UI
{
    public class MainScreenTutorialStepSignal
    {
        public readonly MainScreenWidgetBlockType MainScreenWidgetBlockType;
        public readonly bool IsOnce;

        public MainScreenTutorialStepSignal(MainScreenWidgetBlockType mainScreenWidgetBlockType,bool isOnce = false)
        {
            MainScreenWidgetBlockType = mainScreenWidgetBlockType;
            IsOnce = isOnce;
        }
    }
    
    public enum MainScreenWidgetBlockType
    {
        UpgradesBlock,
        ShopBlock,
        MissionBlock,
        Other,
        OfficersBlock,
    }
}