using System;
using System.Collections.Generic;
using TypeReferences;
using UI;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "UiConfig", menuName = "Installers/UiConfig")]
public class UIConfig : ScriptableObject
{
    [SerializeField] private List<ScreenReference> _uiScreens;
    
    public AssetReference GetUIPrefab<T>() where T : UIScreenController
    {
        foreach (var screen in _uiScreens)
        {
            if (screen.ClassReference.Type == typeof(T))
            {
                return screen.Prefab;
            }
        }
        return null;
    }

    [Serializable]
    public class ScreenReference
    {
        public AssetReference Prefab;

        [Inherits(typeof(UIScreenController))] 
        public TypeReference ClassReference;
    }
}