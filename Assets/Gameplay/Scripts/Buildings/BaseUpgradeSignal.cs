using Gameplay.Configs;

namespace Gameplay.Scripts.Buildings
{
    public class BaseUpgradeSignal
    {
        public string Key { get; private set; }
        public BuildingName BuildingName { get; private set; }

        public BaseUpgradeSignal(string key, BuildingName buildingName )
        {
            Key = key;
            BuildingName = buildingName;
        }
    }
}