using System;
using System.Collections.Generic;
using System.Linq;
using Tutorial;
using TypeReferences;
using UnityEngine;

namespace Gameplay.Scripts.Tutorial
{
    public class TutorialConfig : ScriptableObject
    {
        [SerializeField] private List<TutorialData> _tutorialData = default;
        
        public Type GetTutorialType(TutorialType tutorialType)
        {
            return _tutorialData.First(data => data.TutorialType == tutorialType).TutorialTypeReference;
        }

        [System.Serializable]
        private class TutorialData
        {
            [SerializeField] private TutorialType tutorialType;
            public TutorialType TutorialType => tutorialType;

            [Inherits(typeof(ITutorial))]
            [SerializeField] private TypeReference _tutorialType;
            public TypeReference TutorialTypeReference => _tutorialType;
        }
    }
}