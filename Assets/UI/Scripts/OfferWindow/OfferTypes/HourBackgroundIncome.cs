using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.Signals;
using TMPro;
using UI.Scripts.ShopWindow;
using UI.UIUtils;
using UnityEngine;
using Zenject;

namespace Gameplay.Scripts.IAPController
{
    public class HourBackgroundIncome : ShopOfferBase
    {
        /*[SerializeField] private TextMeshProUGUI _text;
        private SignalBus _signalBus;
        private PlayerPrefsSaveManager _playerPrefsSaveManager;

        [Inject]
        private void Construct(SignalBus signalBus,PlayerPrefsSaveManager playerPrefsSaveManager)
        {
            _playerPrefsSaveManager = playerPrefsSaveManager;
            _signalBus = signalBus;
        }

        public override void Init()
        {
            _signalBus.Subscribe<RefreshShopSignal>(Refresh);
            Refresh();
            base.Init();
        }

        private void Refresh()
        {
            var hourBackground = 1;
            
            /*if (_playerPrefsSaveManager.PrefsData.InAppInfoModel.ExtraTime1BackgroundActivated)
            {
                hourBackground += 1;
            }
            
            if (_playerPrefsSaveManager.PrefsData.InAppInfoModel.ExtraTime3BackgroundActivated)
            {
                hourBackground += 3;
            }
            
            if (_playerPrefsSaveManager.PrefsData.InAppInfoModel.ExtraTime7BackgroundActivated)
            {
                hourBackground += 7;
            }

            if (_playerPrefsSaveManager.PrefsData.InAppInfoModel.ExtraTime12BackgroundActivated)
            {
                hourBackground += 12;
            }
            #1#

            if (hourBackground > 1)
            {
                _text.color = ImageUtils.GetColorByIntRGB(36, 254, 59);
            }

            _text.text = hourBackground + "hour";
        }
        
        public override void Dispose()
        {
            _signalBus.TryUnsubscribe<RefreshShopSignal>(Refresh);
            base.Dispose();
        }*/
    }
}