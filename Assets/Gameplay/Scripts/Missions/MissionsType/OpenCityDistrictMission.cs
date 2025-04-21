using System.Collections.Generic;
using System.Linq;
using Gameplay.Scripts.Buildings;
using UnityEngine;

namespace Gameplay.Scripts.Missions.MissionsType
{
    public class OpenCityDistrictMission : Mission
    {
        public override bool CheckComplete()
        {
            return GetCurrentProgress() >= GetGoal();
        }

        public override int GetCurrentProgress()
        {
            _progress = _prefsSaveManager.PrefsData.LevelInfoModel.GetZoneIndexByLevel(_levelController.CurrentLevel.Level);
            _missionSaveInfo.Progress = _progress;
            return _progress;
        }

        public override int GetGoal()
        {
            return _missionConfigInitializer.NumberOfDistrictOpen-1;
        }
    }
}