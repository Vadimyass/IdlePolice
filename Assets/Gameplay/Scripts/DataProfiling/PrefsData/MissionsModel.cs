using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Scripts.Utils;
using ModestTree;
using UnityEngine;

namespace Gameplay.Scripts.DataProfiling.PrefsData
{
    [Serializable]
    public class MissionsModel : IPlayerPrefsData
    {
        public IReadOnlyDictionary<int, MissionsInfo> LevelMission => _levelMission;
        private Dictionary<int, MissionsInfo> _levelMission = new ();
        
        public IReadOnlyDictionary<int, int> LootedMilestone => _lootedMilestone;
        private Dictionary<int, int> _lootedMilestone = new ();
        
        public void Initialize()
        {

        }

        public MissionsInfo GetMissionsList(int baseId)
        {
            MissionsInfo info;
            if (_levelMission.ContainsKey(baseId) == false)
            {
                info = new MissionsInfo();
                _levelMission.Add(baseId, info);
            }
            
            _levelMission.TryGetValue(baseId, out info);
            return info;
        }

        public bool IsMissionCompleted(int baseId, string missionKey)
        {
            MissionsInfo info;
            if (_levelMission.ContainsKey(baseId) == false)
            {
                return false;
            }
            
            _levelMission.TryGetValue(baseId, out info);
            if (info.CurrentMissions.Any(x => x.Key == missionKey) == true)
            {
                return info.CurrentMissions.First(x => x.Key == missionKey).IsCompleted;
            }

            return false;
        }

        public void ChangeProgressOfMission(int baseId, string missionKey, int progress)
        {
            MissionsInfo info;
            if (_levelMission.ContainsKey(baseId) == false)
            {
                return;
            }
            
            _levelMission.TryGetValue(baseId, out info);
            if (info.CurrentMissions.Any(x => x.Key == missionKey) == true)
            {
                info.CurrentMissions.First(x => x.Key == missionKey).Progress = progress;
            }
            
        }
        
        public bool TryAddCurrentMission(int baseId, string missionKey)
        {
            MissionsInfo info;
            if (_levelMission.ContainsKey(baseId) == false)
            {
                info = new MissionsInfo();
                _levelMission.Add(baseId, info);
            }
            
            _levelMission.TryGetValue(baseId, out info);
            if (info.CurrentMissions.Any(x => x.Key == missionKey) == false)
            {
                var missInfo = new MissionSaveInfo();
                missInfo.Key = missionKey;
                info.CurrentMissions.Add(missInfo);
                return true;
            }

            return false;
        }

        public void CompleteMission(int baseId, string missionKey)
        {
            if (_levelMission.ContainsKey(baseId) == false) return;

            _levelMission.TryGetValue(baseId, out var info);
            if (info.CurrentMissions.Any(x => x.Key == missionKey) == true)
            {
                var mission = info.CurrentMissions.First(x => x.Key == missionKey);
                mission.IsCompleted = true;
            }
        }

        public bool CheckMissionDuplicate(int baseId, string missionKey)
        {
            if (_levelMission.ContainsKey(baseId) == false) return false;
            
            _levelMission.TryGetValue(baseId, out var info);
            
            return info.CurrentMissions.Any(x => x.Key == missionKey);
        }

        public int GetCompletedMissionsCount(int baseID)
        {
            if (_levelMission.ContainsKey(baseID) == false) return 0;
            
            _levelMission.TryGetValue(baseID, out var info);

            return info.CurrentMissions.Count(x => x.IsCompleted);
        }

        public void SetLastLootedMilestone(int baseID, int index)
        {
            if (_lootedMilestone.ContainsKey(baseID) == false)
            {
                _lootedMilestone.Add(baseID, index);
                return;
            }
            
            _lootedMilestone[baseID] = index;
        }
        
        public int GetLastLootedMilestone(int baseID)
        {
            if (_lootedMilestone.ContainsKey(baseID) == false)
            {
                _lootedMilestone.Add(baseID,0);
                return 0;
            }
            
            _lootedMilestone.TryGetValue(baseID, out var info);

            return info;
        }
        
        public void ClearMissions()
        {
            _levelMission = new Dictionary<int, MissionsInfo>();
        }
    }
    
    public class MissionsInfo
    {
        public List<MissionSaveInfo> CurrentMissions = new List<MissionSaveInfo>();
    }

    public class MissionSaveInfo
    {
        public string Key;
        public int Progress;
        public bool IsCompleted;
    }
}