using System;
using System.Collections.Generic;
using Gameplay.Scripts.DataProfiling.PrefsData;
using UnityEngine;

namespace Gameplay.Scripts.Utils
{
    public static class GameTimeUtils
    {
        public static bool IsNowInTimeRange(Vector2Int currentTime, Vector2Int activateTime, Vector2Int endTime)
        {
            var currentTimeFull = currentTime.x * 60 + currentTime.y;
            
            endTime.x = endTime.x == 0 ? 24 : endTime.x;
            
            var activateTimeFull = activateTime.x * 60 + activateTime.y;
            var endTimeFull = endTime.x * 60 + endTime.y;

            return currentTimeFull >= activateTimeFull && currentTimeFull < endTimeFull;
        }

        public static int HowMuchTimePast(Vector2Int currentTime, Vector2Int activateTime)
        {
            var currentTimeFull = currentTime.x * 60 + currentTime.y;
            var activateTimeFull = activateTime.x * 60 + activateTime.y;

            return currentTimeFull - activateTimeFull;
        }

        public static float GetFloatOfCurrentTime(Vector2Int time)
        {
            return (time.x + ((float)time.y) / 60) / 24;
        }

        public static Vector2Int ChangeTimeValue(Vector2Int time, int minutesReduce)
        {
            Vector2Int newTime;
            var difference = time.y - minutesReduce;
            if (difference < 0)
            {
                newTime = new Vector2Int(time.x - 1, 60 - Mathf.Abs(difference));
            }
            else
            {
                newTime = new Vector2Int(time.x, time.y - minutesReduce);
            }

            if (newTime.x < 0)
            {
                newTime.x = 23;
            }
            
            return newTime;
        }

        public static double HowMuchPercentReachedOfDuration(Vector2Int currentTime,Vector2Int startTime,Vector2Int completeTime)
        {
            var currentTimeFull = new TimeSpan(currentTime.x,currentTime.y,0);
            
            completeTime.x = completeTime.x == 0 ? 24 : completeTime.x;
            
            var activateTimeFull = new TimeSpan(startTime.x,startTime.y,0);
            var endTimeFull = new TimeSpan(completeTime.x,completeTime.y,0);

            var duration = endTimeFull.Subtract(activateTimeFull).TotalMinutes;
            var currentProgress = currentTimeFull.Subtract(activateTimeFull).TotalMinutes;
            return currentProgress / duration;
        }

        public static TimeSpan ConvertRealtimeToGameTime(long stampTime,int maxHoursCount,bool withLimit = true)
        {
            var time = DateTimeOffset.FromUnixTimeMilliseconds(stampTime).TimeOfDay.TotalSeconds;
            
            if (time > maxHoursCount * 3600 && withLimit == true)
            {
                time = maxHoursCount * 3600;
            }
            
            return TimeSpan.FromSeconds(time);
        }
        
        public static long ConvertRealtimeToGameTimeLong(long stampTime,int maxHoursCount)
        {
            var time = TimeSpan.FromMilliseconds(stampTime).TotalSeconds;
            
            if (time > maxHoursCount * 3600)
            {
                time = maxHoursCount * 3600;
            }
            
            return (long)time;
        }


        public static bool IsReached30PercentOfDuration(Vector2Int currentTime,Vector2Int startTime,Vector2Int completeTime)
        {
            var currentTimeFull = new TimeSpan(currentTime.x,currentTime.y,0);
            
            completeTime.x = completeTime.x == 0 ? 24 : completeTime.x;
            
            var activateTimeFull = new TimeSpan(startTime.x,startTime.y,0);
            var endTimeFull = new TimeSpan(completeTime.x,completeTime.y,0);

            var duration = endTimeFull.Subtract(activateTimeFull).TotalMinutes;
            var currentProgress = currentTimeFull.Subtract(activateTimeFull).TotalMinutes;
            return currentProgress / duration >= 0.3f;
        }
    }
}