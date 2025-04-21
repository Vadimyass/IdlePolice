using System;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.Scripts.Utils
{
    public class UnityEventsHandler : MonoBehaviour
    {
        public static readonly UnityBoolEvent OnPaused = new UnityBoolEvent();
        public static readonly UnityEvent OnQuit = new UnityEvent();

        public static readonly UnityEvent OnUpdate = new();
        
        [RuntimeInitializeOnLoadMethod]
        static void Init()
        {
            DontDestroyOnLoad(new GameObject("UnityEventsHandler").AddComponent<UnityEventsHandler>());
        }

        private void Update()
        {
            OnUpdate?.Invoke();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            OnPaused?.Invoke(pauseStatus);
        }

        private void OnApplicationQuit()
        {
            OnQuit?.Invoke();
        }

        public class UnityBoolEvent : UnityEvent<bool> { }
    }
}