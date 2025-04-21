using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Scripts.GameBootstrap;
using Gameplay.Scripts.Utils;
using UnityEngine;
using Zenject;

namespace UI.Scripts.LoadingScreen
{
    public class LoadingScreenController : UIScreenController<LoadingScreen>
    {
        private bool _textAnimated;
        public static bool OnLoadingEnded;
        private SignalBus _signalBus;
        
        private float _sliderStep = 0.2f;
        private float _maxSliderValue = 0;


        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public override async UniTask Display(UIArguments arguments)
        {
            _signalBus.TryUnsubscribe<LoadingProgressSignal>(SetValueMax);
            await base.Display(arguments);
            OnLoadingEnded = false;
            View.ProgressSlider.value = 0;
            _signalBus.Subscribe<LoadingProgressSignal>(SetValueMax); 
            UnityEventsHandler.OnUpdate.AddListener(Update);
            
            StartAnimationText();
            StartAnimationCars();
        }

        private void StartAnimationCars()
        {
            for (int i = 0; i < View.CarAgents.Count; i++)
            {
                View.CarAgents[i].transform.DOMove(View.EndPoints[i].position, 3).From(View.StartPoints[i].position)
                    .SetLoops(-1).SetEase(Ease.Linear);
            }
        }
        
        private async void Update()
        {
            if (View.ProgressSlider.value <= _maxSliderValue)
            {
                View.ProgressSlider.value += _sliderStep * Time.deltaTime;
            }

            if (Math.Abs(View.ProgressSlider.value - View.ProgressSlider.maxValue) < 0.002)
            {
                UnityEventsHandler.OnUpdate.RemoveAllListeners();
                _signalBus.TryUnsubscribe<LoadingProgressSignal>(SetValueMax);
                OnLoadingEnded = true;
            }
        }

        private async void StartAnimationText()
        {
            _textAnimated = true;
            while (_textAnimated == true)
            {
                View.TextMeshProUGUI.text = "Loading";
                await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
                View.TextMeshProUGUI.text = "Loading.";
                await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
                View.TextMeshProUGUI.text = "Loading..";
                await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
                View.TextMeshProUGUI.text = "Loading...";
                await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            }
        }
        
        

        public override UniTask OnHide()
        {
            _textAnimated = false;
            return base.OnHide();
        }
        
        public void SetValueMax(LoadingProgressSignal signal)
        {
            var value = signal.Value;
            _maxSliderValue = value;
        }
    }
}