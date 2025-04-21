using System.Collections.Generic;
using TypeReferences;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Gameplay.Scripts.Tutorial
{
    public class TutorialContextObjectConfig : ScriptableObject
    {
        [SerializeField] private List<TutorialObjectReferenceData> _tutorialObjectReferenceData = default;

        public List<TutorialObjectReferenceData> TutorialObjects => _tutorialObjectReferenceData;

        [System.Serializable]
        public class TutorialObjectReferenceData
        {
            [SerializeField] private AssetReference _assetReference;
            public AssetReference AssetReference => _assetReference;
        
            [Inherits(typeof(TutorialContextMonoBehaviourObject))]
            [SerializeField] private TypeReference _tutorialObjectType;

            public TypeReference TutorialObjectType => _tutorialObjectType;
        }
    }
}