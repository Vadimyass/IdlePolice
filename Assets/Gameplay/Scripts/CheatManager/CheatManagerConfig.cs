using System.Collections.Generic;
using TypeReferences;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Gameplay.Scripts.CheatManager
{
    public class CheatManagerConfig : ScriptableObject
    {
        [SerializeField] private AssetReference _cheatWindowReference;
        [SerializeField] private List<AssetReference> _popupsReferences;
        
        [SerializeField] private List<CheatManagerItemData> cheatItemsData = default;

        public AssetReference CheatWindowReference => _cheatWindowReference;
        public List<AssetReference> PopupsReferences => _popupsReferences;
        
        public AssetReference GetReference<T>() where T : CheatItemBase
        {
            foreach (var data in cheatItemsData)
            {
                if (data.CheatItemType.Type == typeof(T))
                {
                    return data.AssetReference;
                }
            }

            return null;
        }
    }
    
    [System.Serializable]
    public class CheatManagerItemData
    {
        [SerializeField] private AssetReference assetReference;
        public AssetReference AssetReference => assetReference;
        
        [Inherits(typeof(CheatItemBase))]
        [SerializeField]private TypeReference cheatItemType;
        public TypeReference CheatItemType => cheatItemType;
    }
}