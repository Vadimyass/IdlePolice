using System;
using System.Collections.Generic;

namespace Gameplay.Scripts.Utils
{
    public static class RegexExtensions
    {
        public static IEnumerable<string> SplitAndKeep(this string s, string seperator,StringSplitOptions splitOptions)
        {
            string[] obj = s.Split(new string[] {seperator}, splitOptions);

            for (int i = 0; i < obj.Length; i++)
            {
                string result = i == obj.Length - 1 ? obj[i] : obj[i] + seperator;
                yield return result;
            }
        }
    }
}