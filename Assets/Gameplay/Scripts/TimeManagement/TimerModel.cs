using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Scripts.Buildings;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.Utils;
using Newtonsoft.Json;
using SolidUtilities.Collections;
using UnityEngine;
using Zenject;

namespace Gameplay.Scripts.TimeManagement
{
    [Serializable]
    public class TimerModel : IPlayerPrefsData
    { 
        public IReadOnlyList<Timer> Timers => _timers;
        [SerializeField] private List<Timer> _timers = new ();
        
        public IReadOnlyList<Timer> OnlineTimers => _onlineTimers;
        [SerializeField] private List<Timer> _onlineTimers = new ();
        
        [SerializeField] private SerializableDictionary<int, Timer> _incomeBuffTimer = new ();

        private bool _onlineTimersTicking;
        public SerializableDictionary<int, Timer> IncomeBuffTimer => _incomeBuffTimer;
        private SignalBus _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<TimeTickSignal>(OnTick);
        }
        
        public void Initialize()
        {
            
        }

        public void DecreaseTimeOfAllTimers(long passedTime)
        {
            foreach (var timer in _timers)
            {
                if (timer.IsEnded == false)
                {
                    timer.DecreaseDuration((int)passedTime);
                }
            }
        }

        /// <param name="duration">In seconds</param>
        private Timer GetTimer(string hashCode, float duration, int baseID)
        {
            Timer timer;
            if (_timers.Any(x => x.IsEnded == true) == true)
            {
                timer = _timers.First(x => x.IsEnded == true);
            }
            else
            {
                timer = new Timer();
                _timers.Add(timer);
            }
            timer.Init(hashCode, duration, baseID);
            return timer;
        }
        
        /// <param name="duration">In seconds</param>
        private Timer GetOnlineTimer(string hashCode, int duration, int baseID)
        {
            Timer timer;
            if (_onlineTimers.Any(x => x.IsEnded == true) == true)
            {
                timer = _onlineTimers.First(x => x.IsEnded == true);
            }
            else
            {
                timer = new Timer();
                _onlineTimers.Add(timer);
            }
            timer.Init(hashCode, duration, baseID);
            return timer;
        }

        public bool TryGetIncomeTimer(int baseID, string hashCode, int duration, out Timer timer)
        {
            _incomeBuffTimer.TryGetValue(baseID, out timer);
            if (timer == null)
            {
                timer = new Timer();
                timer.Init(hashCode, duration, baseID);
                _incomeBuffTimer.Add(baseID, timer);
                return false;
            }
            
            return true;
        }

        public bool IsExistTimer(string hashCode)
        {
            var timer = _timers.Find(x=> x.HashCode == hashCode && x.IsEnded == false);

            return timer != null;
        }

        public bool TryGetTimer(string hashCode, float duration,  out Timer timer, int baseID = 0)
        {
            timer = _timers.Find(x=> x.HashCode == hashCode);
            
            if (timer == null)
            {
                timer = GetTimer(hashCode, duration, baseID);
                return false;
            }

            if (timer.IsEnded == true)
            {
                timer.Init(hashCode, duration, baseID);
            }

            return true;
        }
        
        public bool TryGetOnlineTimer(string hashCode, int duration, out Timer timer, int baseID = 0)
        {
            timer = _onlineTimers.Find(x=> x.HashCode == hashCode);
            
            if (timer == null)
            {
                timer = GetOnlineTimer(hashCode, duration, baseID);
                return false;
            }
            
            if (timer.IsEnded == true)
            {
                 timer.Init(hashCode, duration, baseID);
            }

            return true;
        }
        
        public Timer GetRTSTimer()
        {
            TryGetOnlineTimer("-555555555", 3600, out var timer, -1);
            return timer;
        }

        public bool TryIncreaseTimerDuration(Timer timer, int addDuration, int maxDuration)
        {
            if (timer != null)
            {
                timer.IncreaseDuration(addDuration, maxDuration);
                return true;
            }
            return false;
        }

        private void OnTick()
        {

            var fakeTimers = new List<Timer>();
            foreach (var timer in _timers)
            {
                if (timer == null)
                {
                    fakeTimers.Add(timer);
                    continue;
                }

                if (timer.IsEnded == false)
                {
                    timer.OnTick();
                }
            }

            foreach (var timer in _onlineTimers)
            {
                if (_onlineTimersTicking == true)
                {
                    if (timer.IsEnded == false)
                    {
                        timer.OnTick();
                    }
                }
            }

            foreach (var timerInfo in _incomeBuffTimer)
            {
                var timer = timerInfo.Value;
                if (timer == null)
                {
                    fakeTimers.Add(timer);
                    continue;
                }

                if (timer.IsEnded == false)
                {
                    timer.OnTick();
                }
            }

            foreach (var timer in fakeTimers)
            {
                _timers.Remove(timer);
            }

        }

        public void ChangeForOnlineTimers(bool isCan)
        {
            _onlineTimersTicking = isCan;
        }
    }
}