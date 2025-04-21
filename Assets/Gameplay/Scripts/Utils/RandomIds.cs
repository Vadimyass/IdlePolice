using System;

namespace Gameplay.Scripts.Utils
{
    public static class RandomIds
    {
        public static string GetRandomId()
        {
            return DateTime.Now.Ticks.ToString().Substring(0, 10) + UnityEngine.Random.Range(0, 99999999);
        }
    }
}