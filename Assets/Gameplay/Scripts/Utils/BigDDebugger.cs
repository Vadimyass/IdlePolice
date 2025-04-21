using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Gameplay.Scripts.Utils
{
    public class BigDDebugger
    {
        public static void Log(params object[] objects)
        {
            var message = String.Empty;
            foreach (var obj in objects)
            {
                message += obj + " ";
            }
            Debug.Log(message);
        }
        
        public static void LogWarning(params object[] objects)
        {
            var message = String.Empty;
            foreach (var obj in objects)
            {
                message += obj + " ";
            }
            Debug.LogWarning(message);
        }
        
        public static void LogError(params object[] objects)
        {
            var message = String.Empty;
            foreach (var obj in objects)
            {
                message +=  obj + " ";
            }
            Debug.LogError(message);
        }
        
        public static void LogErrorWithCheck(params object[] objects)
        {
            var message = String.Empty;
            foreach (var obj in objects)
            {
                var text = obj.ToString();
                text = Regex.Replace(text, @"[^\u0000-\u007F]+", "^");
                message += text + " ";
            }
            Debug.LogError(message);
        }
    }
}