using System;
using System.Collections.Generic;
using Audio;
using Cysharp.Threading.Tasks;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Particles;
using UI.UIUtils;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Zenject;

namespace UI.Scripts.ShopWindow
{
    public class ShopWindowController : UIScreenController<ShopWindow>
    {
        private UIManager _uiManager;
        private PlayerPrefsSaveManager _playerPrefsSaveManager;
        private ParticleManager _particleManager;

        [Inject]
        private void Construct(UIManager uiManager,PlayerPrefsSaveManager playerPrefsSaveManager, ParticleManager particleManager)
        {
            _particleManager = particleManager;
            _playerPrefsSaveManager = playerPrefsSaveManager;
            _uiManager = uiManager;
        }
        
        public override void Init(UIScreen uiScreen)
        {
            base.Init(uiScreen);
            
            foreach (var currencyContainer in View.CurrencyContainers)
            {
                _particleManager.AddCurrencyContainerForShop(currencyContainer.CurrencyType, currencyContainer);
            }

            foreach (var sectionsAndButton in View.ShopSectionsAndButtons)
            {
                sectionsAndButton.Key.onClick.AddListener((() => View.ScrollRect.FocusOnItemCoroutine(sectionsAndButton.Value as RectTransform, 2)));
            }
            
            View.CloseButton.onClick.AddListener(() =>
            {
                _audioManager.PlaySound(TrackName.ButtonClick);
                _uiManager.HideLast();
                //View.Offers.ForEach(x => x.Dispose());
            });

            /*foreach (var timeMachineOffer in View.TimeMachineOffers)
            {
                timeMachineOffer.Init();
            }*/
            
            foreach (var gachaBoxOffer in View.GachaBoxOffers)
            {
                gachaBoxOffer.Init();
            }

            View.CrystalContainer.Init();
        }
        
        public override async UniTask Display(UIArguments arguments)
        { 
            base.Display(arguments);
            Time.timeScale = 1;
            var args = (ShopWindowArguments)arguments;
            
            /*foreach (var timeMachineOffer in View.TimeMachineOffers)
            {
                timeMachineOffer.Refresh();
            }*/
            
            foreach (var gachaBoxView in View.GachaBoxOffers)
            {
                gachaBoxView.Refresh();
            }
            
            //View.Offers.ForEach(x => x.Init());
            
            await UniTask.Delay(TimeSpan.FromSeconds(0.25f));
            foreach (var rectTransform in View.Rebuildables)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            }
            Canvas.ForceUpdateCanvases();
            
            //View.ScrollRect.FocusOnItem(View.OfferFocuses[args.OfferType] as RectTransform);
            
            foreach (var currencyContainer in View.CurrencyContainers)
            {
                currencyContainer.Refresh();
            }
        }
    }
}