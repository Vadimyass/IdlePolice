using System.Linq;
using System.Text.RegularExpressions;
using MyBox;
using UnityEngine;

namespace Gameplay.Scripts.Utils
{
    public static class StringUtils
    {
        public static string AdaptBuildingKeyToLocalizationKey(string buildingKey)
        {
            var key = buildingKey.Split("_").FirstOrDefault();
            if (key?.Last() == '_')
            {
                key = new string(key.Take(key.Length - 1).ToArray());
            }
            return "[Building_name_" + key + "]";
        }
        
        public static string AdaptBuildingKeyToDescriptionKey(string buildingKey)
        {
            var key = buildingKey.Split("_").FirstOrDefault();
            if (key?.Last() == '_')
            {
                key = new string(key.Take(key.Length - 1).ToArray());
            }
            return "[Building_name_" + key + "_txt]";
        }
        
        public static string AdaptUpgradableKeyToLocalizationKey(string buildingKey)
        {
            var key = Regex.Split(buildingKey, @"\d+").FirstOrDefault();
            if (key?.Last() == '_')
            {
                key = new string(key.Take(key.Length - 1).ToArray());
            }
            return "[Upgradable_name_" + key + "]";
        }
        
        public static string AdaptUpgradableKeyToDescriptionKey(string buildingKey)
        {
            var key = Regex.Split(buildingKey, @"\d+").FirstOrDefault();
            return "[Upgradable_name_" + key + "txt]";
        }

        public static string RemoveSpecificCharsFromString(string str,string[] chars)
        {
            var filtredString = string.Empty;
            
            foreach (var symbol in chars)
            {
                Debug.LogError(symbol);
                filtredString = str.Replace(symbol, string.Empty);
            }

            return filtredString;
        }
    }
}