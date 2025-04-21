using System;

namespace Gameplay.Scripts.Utils
{
    public static class DateTimeExtensions
        {
            public static DateTime ChangeTime(DateTime dateTime, int hours, int minutes = 0 , int seconds = 0, int milliseconds = 0)
            {
                return new DateTime(
                    dateTime.Year,
                    dateTime.Month,
                    dateTime.Day,
                    hours,
                    minutes,
                    seconds,
                    milliseconds,
                    dateTime.Kind);
            }
        }
}