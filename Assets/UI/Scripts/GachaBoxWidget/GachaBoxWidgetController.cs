using System.Collections.Generic;
using Audio;
using BigD.Config;
using Cysharp.Threading.Tasks;
using Gameplay.Configs;
using Gameplay.Scripts.DataProfiling.Models;
using Gameplay.Scripts.DataProfiling.PrefsData;
using UI.Scripts.GachaBoxOpenWindow;
using UI.Scripts.ShopWindow.Gacha;
using Zenject;

namespace UI.Scripts.GachaBoxWidget
{
    public class GachaBoxWidgetController : UIScreenController<GachaBoxWidget>
    {
        private GachaBoxWidgetArguments _args;
        private PlayerPrefsSaveManager _playerPrefsSaveManager;
        private GameConfig _gameConfig;
        private BoxesConfigInitializator _info;
        private UIManager _uiManager;
        private LocalizationManager _localizationManager;

        private const string AvailableKey = "[AVAILABLE]";

        [Inject]
        private void Construct(UIManager uiManager,  PlayerPrefsSaveManager playerPrefsSaveManager, GameConfig gameConfig, LocalizationManager localizationManager)
        {
            _localizationManager = localizationManager;
            _uiManager = uiManager;
            _gameConfig = gameConfig;
            _playerPrefsSaveManager = playerPrefsSaveManager;
        }

        public override void Init(UIScreen uiScreen)
        {
            base.Init(uiScreen);
            View.OpenButton.onClick.AddListener(OpenBox);
            View.CloseButton.onClick.AddListener((() =>
            {
                _audioManager.PlaySound(TrackName.ButtonClick);
                _uiManager.HideCurrentWidget();
            }));
        }

        private async void OpenBox()
        {
            var count = _playerPrefsSaveManager.PrefsData.ConsumablesInfoModel.GetCountOfGachaBox(_args.GachaBoxType);
            count = count > 10 ? 10 : count;
            if (_playerPrefsSaveManager.PrefsData.ConsumablesInfoModel.TryConsumeGachaBox(_args.GachaBoxType, count))
            {
                View.OpenButton.interactable = false;
                var args = new GachaBoxOpenWindowArguments(_args.GachaBoxType, _args.NameKey, _args.Sprite, count);
                _uiManager.HideCurrentWidget();
                await _uiManager.Show<GachaBoxOpenWindowController>(args);
                View.OpenButton.interactable = true;

                //SendAnalytics(_args.GachaBoxType);
            }
        }

        public override async UniTask Display(UIArguments arguments)
        {
            await base.Display(arguments);
            _args = (GachaBoxWidgetArguments)arguments;
            _info = _gameConfig.BoxesInfoConfig.GetItemByKey(_args.GachaBoxType);

            //View.NameText.text = _localizationManager.TryTranslate(_args.NameKey);
            View.BoxImage.sprite = _args.Sprite;
            
            View.BasicCardsText.text = _info.RareChance.ToString();
            View.SilverCardsText.text = _info.EpicChance.ToString();
            View.GolderCardsText.text = _info.LegendaryChance.ToString();
            //View.AvailableBoxText.text = _localizationManager.TryTranslate(AvailableKey) + ": " + _playerPrefsSaveManager.PrefsData.ConsumablesInfoModel.GetCountOfGachaBox(_args.GachaBoxType);
        }


        /*private void SendAnalytics(GachaBoxType gachaBoxType)
        {
            AnalyticsManager.LogAppMetricaEvent(EventMetricaName.chest_open,true,(EventMetricaParameterName.chest_type,gachaBoxType));
        }*/
        
    }
}