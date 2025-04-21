using Audio;
using Cysharp.Threading.Tasks;
using Gameplay.Configs;
using Gameplay.Scripts.Utils;
using UI.Scripts.ShopWindow;
using Zenject;

namespace UI.Scripts.OfferWindow
{
    public class OfferWindowController : UIScreenController<OfferWindow>
    {
        private UIManager _uiManager;
        private OfferData _info;
        private LocalizationManager _localizationManager;
        private OfferWindowArguments _args;

        [Inject]
        private void Construct(UIManager uiManager, LocalizationManager localizationManager)
        {
            _localizationManager = localizationManager;
            _uiManager = uiManager;
        }
        
        public override void Init(UIScreen uiScreen)
        {
            base.Init(uiScreen);
            View.CloseButton.onClick.AddListener(DeclineOffer);
        }

        public override async UniTask Display(UIArguments arguments)
        {
            await base.Display(arguments);
            _args = (OfferWindowArguments)arguments;

            foreach (var transform in View.Offers)
            {
                transform.Value.gameObject.SetActive(false);
            }

            View.OfferButton.onClick.RemoveAllListeners();
            
            View.Offers.TryGetValue(_args.OfferType, out var offer);
            if (offer != null)
            {
                offer.gameObject.SetActive(true);
            }
            
            View.OfferButton.onClick.AddListener(OnTryBuy);

            _info = View.OfferConfig.GetDataByType(_args.OfferType);

            View.NameText.text = _localizationManager.TryTranslate(_info.NameKey);
            View.DescriptionText.text = _localizationManager.TryTranslate(_info.DescriptionKey);
            //AnalyticsManager.LogEvent("offer_" + _info.AnalyticKey + "_open");
        }

        private async void DeclineOffer()
        {
            _audioManager.PlaySound(TrackName.ButtonClick);
            //AnalyticsManager.LogEvent("offer_" + _info.AnalyticKey + "_close");
            
            if (_args.WithNextOnDecline)
            {
                await OnHide();
                Display(new OfferWindowArguments(OfferType.ForBeginners));
                return;
            }
            
            _uiManager.HideLast();
        }
        
        private async void OnTryBuy()
        {
            View.OfferButton.interactable = false;
            //AnalyticsManager.LogEvent("offer_" + _info.AnalyticKey + "_buy");
            var args = new ShopWindowArguments(_args.OfferType);
            _uiManager.HideLast();
            await _uiManager.Show<ShopWindowController>(args);
            View.OfferButton.interactable = true;
        }
    }
}