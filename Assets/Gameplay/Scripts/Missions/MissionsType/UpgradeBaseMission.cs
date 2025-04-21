using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.Scripts.Missions.MissionsType
{
    public class UpgradeBaseMission : Mission
    {
        public override bool CheckComplete()
        {
            return GetCurrentProgress() >= GetGoal();
        }

        public override int GetCurrentProgress()
        {
            var hasUpgrade =
                _levelController.CurrentLevel.BaseUpgradesController.HasUpgrade(_missionConfigInitializer
                    .TargetUpgradeName);
            
            _progress = hasUpgrade == true ? 1 : 0;
            _missionSaveInfo.Progress = _progress;
            return _progress;
        }

        public override int GetGoal()
        {
            return 1;
        }
    }
}