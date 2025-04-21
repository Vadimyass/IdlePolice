using Gameplay.Scripts.GameBootstrap;
using Gameplay.Scripts.Utils;
using MyBox;
using UnityEngine;
using Zenject;

namespace Gameplay.Playables.Signals
{
    public class SceneLoaderSignalReceiver : MonoBehaviour
    {
        [Scene][SerializeField] private string _gameplayScene;
        private SignalBus _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        public void UnloadPreviousScene()
        {
            FireSignal(1);
        }
        
        public void LoadScene()
        {
            SceneManagement.LoadScene(_gameplayScene);
        }
        
        private void FireSignal(float value)
        {
            _signalBus.Fire(new LoadingProgressSignal(value));
        }
    }
}