using System;
using Gameplay.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace Gameplay.Scripts.Tutorial
{
    [RequireComponent(typeof(TutorialTargetSingletonResolver))]
    public abstract class TutorialTarget : MonoBehaviour
    {
        public abstract Vector2 CalculateSize();
        public abstract Vector3 CalculatePosition();
        
        protected Lazy<Camera> MainCamera = new (() => Camera.main);
        
        [SerializeField] private string _tagString;
        [SerializeField] private bool _deactivateOnStart;
        [SerializeField] private bool _deactivateOnFinish;

        public bool DeactivateOnFinish => _deactivateOnFinish;
        private string _parsedTag = null;

        private void Start()
        {
            if (_deactivateOnStart)
            {
                gameObject.SetActive(false);
            }
        }

        [CanBeNull] private string GetParsedTag()
        {
            if (string.IsNullOrEmpty(_tagString)) return null;

            if (string.IsNullOrEmpty(_parsedTag))
            {
                if (!TryConvertStringReflection(_tagString, out _parsedTag))
                {
                    _parsedTag = _tagString;
                }                
            }

            return _parsedTag;
        }

        /// <summary>
        /// Trying to convert string to property
        /// </summary>
        /// <param name="inString">input pattern string</param>
        /// <param name="resultString">result of evaluation string</param>
        private static bool TryConvertStringReflection(string inString, out string resultString)
        {
            resultString = default;

            resultString = RuntimeEvaluator.EvaluateProperty(inString);
            
            return !string.IsNullOrEmpty(resultString);
        }

        public bool HaveTag(string tutorialTag) => GetParsedTag()?.Equals(tutorialTag) ?? false;
    }
}