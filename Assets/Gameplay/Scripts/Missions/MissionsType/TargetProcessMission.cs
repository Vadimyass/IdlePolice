using System.Collections.Generic;
using System.Linq;
using Gameplay.Scripts.Buildings;
using Gameplay.Scripts.Utils;
using UnityEngine;

namespace Gameplay.Scripts.Missions.MissionsType
{
    public class TargetProcessMission : Mission
    {
        public override bool CheckComplete()
        {
            return GetCurrentProgress() >= GetGoal();
        }

        public override int GetCurrentProgress()
        {
            _missionSaveInfo.Progress = _progress;
            return _progress;
        }

        public bool TryIncreaseProgress(BuildingProcessName buildingProcessName)
        {
            if (buildingProcessName != _missionConfigInitializer.ProcessBuildingValue.Key) 
                return false;
            
            _progress++;
            GetCurrentProgress();
            return true;
        }
        
        public override int GetGoal()
        {
            return _missionConfigInitializer.ProcessBuildingValue.Value;
        }
    }
}