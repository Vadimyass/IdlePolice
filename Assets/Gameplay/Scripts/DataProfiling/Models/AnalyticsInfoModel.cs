/*using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Analytics;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Newtonsoft.Json;
using UnityEngine;

namespace Gameplay.Scripts.DataProfiling.Models
{
    public class AnalyticsInfoModel : IPlayerPrefsData
    {
        public float TotalMinutes => _totalMinutes;
        
        [JsonProperty] private float _totalMinutes;

        public float TotalMinutesOnBase => _totalMinutesOnBase;
        [JsonProperty] private float _totalMinutesOnBase;

        public int SessionCount => _sessionCount;
        [JsonProperty] private int _sessionCount;

        [JsonProperty] private Dictionary<StageNames, int> _stagesProgress = new ();

        public StageNames CurrentStage = StageNames.none;
        [JsonIgnore] public int MinutesInSession => _minutesInSession;
        [JsonIgnore] private int _minutesInSession = 0;

        [JsonIgnore] private int _currentTutorialStep = 0;
        public int CurrentTutorialStep => _currentTutorialStep;

        public void Initialize()
        {
        }

        public void AddSession()
        {
            _sessionCount++;
        }

        public void SetCurrentTutorialStep(int step)
        {
            _currentTutorialStep = step;
        }

        public void ResetTimeOnBase()
        {
            _totalMinutesOnBase = 0;
        }

        public void AddActiveStage(StageNames stageName)
        {
            _stagesProgress.TryAdd(stageName,0);
            CurrentStage = stageName;
        }

        public int RemoveActiveStage(StageNames stageName)
        {
            if (_stagesProgress.TryGetValue(stageName, out var progress))
            {
                _stagesProgress.Remove(stageName);
                return progress;
            }

            return 0;
        }
        public void TryAddMinuteToStages()
        {
            var stagesKeys = _stagesProgress.Keys.ToArray();
            for (int i = 0; i < stagesKeys.Length; i++)
            {
                if(_stagesProgress.ContainsKey(stagesKeys[i]))
                {
                    _stagesProgress[stagesKeys[i]]++;
                }
            }
        }

        public void SendAnalytics()
        {
            _totalMinutes++;
            _minutesInSession++;
            _totalMinutesOnBase++;
            TryAddMinuteToStages();
            AnalyticsManager.LogAppMetricaEvent(EventMetricaName.playtime_min,false,(EventMetricaParameterName.time,_totalMinutes),(EventMetricaParameterName.session_number,_sessionCount),(EventMetricaParameterName.session_duration,_minutesInSession));
        }
    }
}*/