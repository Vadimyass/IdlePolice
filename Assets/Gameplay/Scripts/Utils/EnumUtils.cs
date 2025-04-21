using System;
using UnityEngine;

namespace Gameplay.Scripts.Utils
{
    public static class EnumExtensions
    {
        public static T Next<T>(this Enum e)
        {
            try
            {
                var enumInfo = Enum.GetValues(typeof(T));
                var currentValue = e.ToString();
                for (int a = 0; a < enumInfo.Length; a++)
                {
                    if (((T)enumInfo.GetValue(a)).ToString() == currentValue.ToString())
                    {
                        if (a == enumInfo.Length - 1)
                        {
                            return (T)enumInfo.GetValue(0);
                        }
                        else
                        {
                            return (T)enumInfo.GetValue(a + 1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Error while read enum: " + ex.Message);
            }

            return default(T);
        }
    }
}