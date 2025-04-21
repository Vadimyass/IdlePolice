namespace Gameplay.Scripts.Buildings
{
    public class ProcessFinishSignal
    {
        public BuildingProcessName BuildingProcessName { get; private set; }

        public ProcessFinishSignal(BuildingProcessName buildingProcessName)
        {
            BuildingProcessName = buildingProcessName;
        }
    }
}