using System;
using Gameplay.Scripts.Buildings;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Scripts.TimeManagement
{
    public class Timer : IDisposable
    {
        public int MilitaryBaseID { get; set; }
        public string HashCode { get;  set; }
        public float Duration { get;  set; } // in seconds
        public float StartDuration { get;  set; }
        public float FullStartDuration { get;  set; }
        public bool IsEnded { get;  set; }
        public float CurrentDecreaseOfTimer { get; set; }

        [NonSerialized] public Action<float> OnTickAction;
        [NonSerialized] public Action<string> OnEndAction;

        /// <param name="duration">In seconds</param>
        public void Init(string hashCode, float duration, int militaryBaseId)
        {
            MilitaryBaseID = militaryBaseId;
            StartDuration = duration;
            Duration = StartDuration;
            FullStartDuration = StartDuration;
            HashCode = hashCode;
            IsEnded = false;
        }
        
        public void OnTick()
        {
            Duration--;
            OnTickAction?.Invoke(1-(float)Duration/StartDuration);
            if (Duration <= 0)
            {
                EndTimer();
            }
        }

        public void DecreaseDuration(int currency)
        {
            Duration -= currency;
            if (Duration <= 0)
            {
                EndTimer();
            }
        }

        public void IncreaseDuration(int addDuration, int maxDuration)
        {
            Duration = Duration < 0 ? 0 : Duration;
            IsEnded = false;
            Duration = Duration + addDuration > maxDuration ? maxDuration : Duration + addDuration;
            StartDuration = maxDuration;
            FullStartDuration = maxDuration;
        }
        
        public void ChangeDurationOnPercents(float percent)
        {
            CurrentDecreaseOfTimer = percent;
            
            if (CurrentDecreaseOfTimer > 50)
            {
                CurrentDecreaseOfTimer = 50;
            }
            if (CurrentDecreaseOfTimer < 0)
            {
                CurrentDecreaseOfTimer = 0;
            }

            var ratio = (float)Duration / StartDuration;
            StartDuration = (int)(FullStartDuration * (1 - CurrentDecreaseOfTimer / 100));
            Duration = (int)(StartDuration * ratio);
        }
        
        public void ChangeStartDuration(int duration)
        {
            var ratio = (float)Duration / StartDuration;
            FullStartDuration = duration;
            StartDuration = (int)(FullStartDuration * (1 - CurrentDecreaseOfTimer / 100));
            Duration = (int)(StartDuration * ratio);
        }

        public void EndTimer()
        {
            IsEnded = true;
            OnEndAction?.Invoke(HashCode);
        }

        public void ReloadTimer(int baseID)
        {
            MilitaryBaseID = baseID;
            Duration = StartDuration;
            FullStartDuration = StartDuration;
            CurrentDecreaseOfTimer = 0;
            IsEnded = false;
        }
        
        public void Dispose()
        {
        }
    }
}