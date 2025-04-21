using Gameplay.Configs;

namespace Gameplay.Scripts.Missions
{
    public class BuildBuildingSignal
    {
        public BuildingName BuildingName { get; private set; }

        public BuildBuildingSignal(BuildingName buildingName)
        {
            BuildingName = buildingName;
        }
    }
}