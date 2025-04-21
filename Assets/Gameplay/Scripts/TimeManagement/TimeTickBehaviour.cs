using UnityEngine;
using Zenject;

namespace Gameplay.Scripts.TimeManagement
{
    class TimeTickBehaviour : MonoBehaviour
    {
        private float _lastTick = 0;
        private TimeTickSignal _timeTickSignal;

        [Inject] private SignalBus _signalBus;
        
        private void Start()
        {
            _timeTickSignal = new TimeTickSignal();
            _lastTick = Time.unscaledTime;
        }

        private void Update()
        {
            if ((Time.unscaledTime - _lastTick) >= 1/Time.timeScale)
            {
                _timeTickSignal.SecondsPast =  Time.timeScale <= 0 ? 1 : (int)Time.timeScale;
                _signalBus.Fire(_timeTickSignal);
                
                _lastTick = Time.unscaledTime;
            }
        }
    }
}