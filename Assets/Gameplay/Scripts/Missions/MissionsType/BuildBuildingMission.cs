using System.Collections.Generic;
using Gameplay.Scripts.Buildings;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.Utils;
using UnityEngine;

namespace Gameplay.Scripts.Missions.MissionsType
{
    public class BuildBuildingMission : Mission
    {
        public override bool CheckComplete()
        {
            BigDDebugger.LogError(GetCurrentProgress() >= GetGoal());
            return GetCurrentProgress() >= GetGoal();
        }

        public override int GetCurrentProgress()
        {
            var buildings = _levelController.CurrentLevel.GetBuiltBuildingByKey(_missionConfigInitializer.BuildBuildingValue.Key);
            _progress = buildings.Count;
            _missionSaveInfo.Progress = _progress;
            return _progress;
        }

        public override int GetGoal()
        {
            return _missionConfigInitializer.BuildBuildingValue.Value;
        }
    }
}