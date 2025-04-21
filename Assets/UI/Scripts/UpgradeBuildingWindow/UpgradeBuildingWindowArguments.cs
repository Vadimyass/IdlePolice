using Gameplay.Scripts.Buildings;

namespace UI.Scripts.UpgradeBuildingWindow
{
    public class UpgradeBuildingWindowArguments : UIArguments
    {
        public Building Building { get; private set; }

        public UpgradeBuildingWindowArguments(Building building)
        {
            Building = building;
        }
    }
}