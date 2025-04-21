using System;
using System.Collections.Generic;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.Tutorial;
using Newtonsoft.Json;
using Tutorial;
using UnityEngine;

namespace Gameplay.Scripts.DataProfiling.Models
{
    public class TutorialModel : IPlayerPrefsData
    {
        [JsonIgnore] private Action _tutorialCompleteSubject = delegate {  };
        [JsonIgnore] public Action TutorialCompleteAsObservable => _tutorialCompleteSubject;
        
        [JsonIgnore] public List<TutorialType> TutorsData => _tutorsData;
        [JsonProperty] private List<TutorialType> _tutorsData = new List<TutorialType>();

        public bool IsTutorialSkip => _isTutorialSkip;
        
        private bool _isTutorialSkip = false;
        
        [JsonIgnore] public bool IsTutorialSkipped => _isTutorialSkipped;
        
        [JsonProperty] private bool _isTutorialSkipped;
        public void Initialize()
        {
            _tutorsData ??= new List<TutorialType>();
        }

        public void SetTutorialSkip(bool isSkip)
        {
            _isTutorialSkip = isSkip;
        }
        
        public void SetTutorialSkipped(bool isSkipped)
        {
            _isTutorialSkipped = isSkipped;
        }
        
        public bool SetTutorCompleted(TutorialType tutor)
        {
            var canComplete = !_tutorsData.Contains(tutor);
            if (canComplete)
            {
                _tutorsData.Add(tutor);
                _tutorialCompleteSubject?.Invoke();
            }

            return canComplete;
        }

        public bool IsContextTutorDone(TutorialType tutor)
        {
            var isDone = IsTutorialSkip || _tutorsData.Contains(tutor);
            return isDone;
        }
    }
}