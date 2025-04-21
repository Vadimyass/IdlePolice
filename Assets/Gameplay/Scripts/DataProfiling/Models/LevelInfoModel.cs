using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.LockedZoneManagement;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace Gameplay.Scripts.DataProfiling.Models
{
    public class LevelInfoModel : IPlayerPrefsData
    {
        [JsonProperty] private int _currentLevelIndex = 0;
        public int CurrentLevelIndex => _currentLevelIndex;
        
        [JsonProperty] private int _openedLevelIndex = 0;
        public int OpenedLevelIndex => _openedLevelIndex;

        [JsonProperty] private List<int> _currentZoneIndex;
        private SignalBus _signalBus;
        
        [JsonProperty] private Dictionary<int,List<BuildingSaveData>> _buildingSaveData = new();

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        public void Initialize()
        {
            if (_currentZoneIndex == null)
            {
                _currentZoneIndex = new List<int>()
                {
                    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                };
            }
        }

        public BuildingSaveData GetBuildingData(int levelIndex, string key, out bool firstTime)
        {
            firstTime = false;

            if (!_buildingSaveData.TryGetValue(levelIndex, out var buildingSaveData))
            {
                firstTime = true;
                buildingSaveData = new List<BuildingSaveData>();
                _buildingSaveData[levelIndex] = buildingSaveData;
            }

            var existingData = buildingSaveData.FirstOrDefault(x => x.Key == key);
            if (existingData != null)
            {
                return existingData;
            }

            firstTime = true;
            var newBuildingData = new BuildingSaveData()
            {
                Key = key,
                IsBuilt = false,
                Level = 1,
                OfficerKey = String.Empty,
            };

            buildingSaveData.Add(newBuildingData);
            return newBuildingData;
        }

        public void ClearBuildingInfo()
        {
        }
        
        public void OpenAccessForLevel(int level)
        {
            _openedLevelIndex = level;
        }

        public void SetBuildingData(int levelIndex, BuildingSaveData buildingSaveData)
        {
            if (!_buildingSaveData.TryGetValue(levelIndex, out var currentBuildingSaveData))
            {
                currentBuildingSaveData = new List<BuildingSaveData>();
                _buildingSaveData[levelIndex] = currentBuildingSaveData;
            }

            var existingDataIndex = currentBuildingSaveData.FindIndex(x => x.Key == buildingSaveData.Key);
            if (existingDataIndex >= 0)
            {
                currentBuildingSaveData[existingDataIndex] = buildingSaveData;
            }
            else
            {
                currentBuildingSaveData.Add(buildingSaveData);
            }
        }

        public void SetCurrentLevelIndex(int index)
        {
            _currentLevelIndex = index;
        }

        public int GetZoneIndexByLevel(int levelIndex)
        {
            return _currentZoneIndex[levelIndex];
        }

        public void SetCurrentZoneIndex(int zoneIndex, int levelIndex)
        {
            _currentZoneIndex[levelIndex] = zoneIndex;
            _signalBus.Fire<ZoneUnlockSignal>();
        }
    }
}