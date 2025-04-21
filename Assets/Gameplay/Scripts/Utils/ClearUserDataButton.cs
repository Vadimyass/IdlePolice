#if UNITY_EDITOR
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.DataProfiling.PrefsData;
using UnityEditor;
using UnityEngine;

namespace Gameplay.Scripts.Utils
{
    public static class ClearUserDataButton
    {
        [MenuItem("Tools/ResetAgents")]
        public static void DeleteData()
        {
            PlayerPrefsSaveManager.DeleteData();
        }
    }
}
#endif