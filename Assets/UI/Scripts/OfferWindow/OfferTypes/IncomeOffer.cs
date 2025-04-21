using DG.Tweening;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.Signals;
using TMPro;
using UI.Scripts.ShopWindow;
using UI.UIUtils;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Scripts.IAPController
{
    public class IncomeOffer : ShopOfferBase
    {
        /*
        [SerializeField] private TextMeshProUGUI _text;
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
            var income = 0;
            /*if (_playerPrefsSaveManager.PrefsData.InAppInfoModel.Extra50IncomeActivated)
            {
                income += 50;
            }
            
            if (_playerPrefsSaveManager.PrefsData.InAppInfoModel.Extra100IncomeActivated)
            {
                income += 100;
            }#1#
            
            if (income >= 50)
            {
                _text.color = ImageUtils.GetColorByIntRGB(36, 254, 59);
            }
            
            _text.text = income + "%";
        }

        public override void Dispose()
        {
            _signalBus.TryUnsubscribe<RefreshShopSignal>(Refresh);
            base.Dispose();
        }
    */
    }
}