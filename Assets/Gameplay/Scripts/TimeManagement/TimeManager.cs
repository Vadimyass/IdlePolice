using System;
using Cysharp.Threading.Tasks;
using Gameplay.Scripts.Buildings;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.Utils;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Gameplay.Scripts.TimeManagement
{
    public class TimeManager : MonoBehaviour
    {
        private PlayerPrefsSaveManager _playerPrefsSaveManager;

        public long ApplicationPauseTimestamp { get; private set; }

        public long ApplicationPausePassedTimestamp { get; private set; }

        public Action<long> OnApplicationResumed;
        private UIManager _uiManager;

        private bool _isInit = false;
        private SignalBus _signalBus;

        private float _delayForSave = 30;
        private float _delayForSendAnalytics = 0;

        [Inject]
        private void Construct(PlayerPrefsSaveManager playerPrefsSaveManager,UIManager uiManager,SignalBus signalBus)
        {
            _signalBus = signalBus;
            _uiManager = uiManager;
            _playerPrefsSaveManager = playerPrefsSaveManager;
        }

        public void Init()
        {
            RestorePauseTime();
            InvokeResumeTime(false);
            
            _signalBus.Subscribe<TimeTickSignal>(OnTick);
        }

        private void OnTick(TimeTickSignal tickSignal)
        {
            _delayForSave += tickSignal.SecondsPast;
            _delayForSendAnalytics += tickSignal.SecondsPast;
            if (_delayForSave >= 3)
            {
                SavePauseTime();
                _delayForSave = 0;
            }

            if (_delayForSendAnalytics >= 60)
            {
                _playerPrefsSaveManager.ForceAnalyticsSave();
                _delayForSendAnalytics = 0;
            }
        }


        private void OnApplicationPause(bool pause)
        {
            if(_isInit == false) return;
            
            if(pause == false)
            {
                RestorePauseTime();
                InvokeResumeTime(true);
            }
        }

        private void OnDestroy()
        {
            SavePauseTime();
            _signalBus.TryUnsubscribe<TimeTickSignal>(OnTick);
        }

        private void RestorePauseTime()
        {
            
        }

        public void SavePauseTime()
        {
            var timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            _playerPrefsSaveManager.ForceSaveTime();
        }

        private async void InvokeResumeTime(bool isFromPause)
        {
            CalculatePassedTimeStamp();
            var passedTimestamp = ApplicationPausePassedTimestamp;
            var passedTimestampTotalMinutes = DateTimeOffset.FromUnixTimeMilliseconds(passedTimestamp).TimeOfDay.TotalMinutes;

            if (passedTimestampTotalMinutes >= 60 && isFromPause == true)
            {
                await _uiManager.HideLast();
            }

            if (passedTimestamp > 0L)
            {
                OnApplicationResumed?.Invoke(passedTimestamp);
            }

            _isInit = true;
        }

        private void CalculatePassedTimeStamp()
        {
            var timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var pauseTimestamp = ApplicationPauseTimestamp;
            if (pauseTimestamp > 0)
            {
                var passedTimestamp = timestamp - pauseTimestamp;
                if (passedTimestamp < 0L)
                {
                    passedTimestamp = 0L;
                }
                ApplicationPausePassedTimestamp = passedTimestamp;
                return;
            }
            ApplicationPausePassedTimestamp = 0L;
        }
        
#if UNITY_EDITOR
        private void OnApplicationFocus(bool hasFocus)
        {
            OnApplicationPause(!hasFocus);
        }
#endif
    }
}