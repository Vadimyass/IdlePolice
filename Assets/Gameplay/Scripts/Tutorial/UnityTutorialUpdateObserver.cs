using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Scripts.Tutorial
{
    public class UnityTutorialUpdateObserver : MonoBehaviour , ITutorialUpdateLoop
    {
        private readonly List<ITutorialUpdateObserver> _updateObservers = new List<ITutorialUpdateObserver>();
        
        private void Update()
        {
            for (var i = 0; i < _updateObservers.Count; i++)
            {
                var observer = _updateObservers[i];
                
                if (observer.TryUpdate()) continue;

                //Process case when object not cleaned yet
                if (observer == _updateObservers[i])
                {
                    _updateObservers.RemoveAt(i);
                }

                i--;
            }
        }

        public void AddTutorialObserver(ITutorialUpdateObserver updateObserver)
        {
            _updateObservers.Add(updateObserver);
        }

        public void RemoveTutorialObserver(ITutorialUpdateObserver updateObserver)
        {
            _updateObservers.Remove(updateObserver);
        }
    }
}