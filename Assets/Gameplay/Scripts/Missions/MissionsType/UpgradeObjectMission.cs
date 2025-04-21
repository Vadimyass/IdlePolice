using System.Collections.Generic;
using System.Linq;
using Gameplay.Scripts.Utils;
using UnityEngine;

namespace Gameplay.Scripts.Missions.MissionsType
{
    public class UpgradeObjectMission : Mission
    {
        public override bool CheckComplete()
        {
            return GetCurrentProgress() >= GetGoal();
        }

        public override int GetCurrentProgress()
        {
            var building = _levelController.CurrentLevel.GetBuiltBuildingByKey(_missionConfigInitializer.UpgradeBuildingValue.Key)
                .FirstOrDefault();

            if (building == null)
            {
                return 0;
            }

            _progress = building.Level;
            _missionSaveInfo.Progress = _progress;
            return _progress;
        }

        public override int GetGoal()
        {
            return _missionConfigInitializer.UpgradeBuildingValue.Value;
        }
    }
}