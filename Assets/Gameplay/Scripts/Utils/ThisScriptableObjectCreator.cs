#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Gameplay.Scripts.Utils
{
    public class ThisScriptableObejctCreator : UnityEditor.Editor
    {
        [MenuItem("Assets/Create/ThisScriptableObject", false,1)]
        public static void CreateManager()
        {
            ScriptableObject asset = ScriptableObject.CreateInstance(Selection.activeObject.name);
            AssetDatabase.CreateAsset(asset, AssetDatabase.GetAssetPath(Selection.activeObject).Replace(".cs", ".asset"));
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    }
}
#endif