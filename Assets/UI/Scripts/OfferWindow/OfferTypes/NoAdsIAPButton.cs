using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.Scripts.IAPController
{
    public class NoAdsIAPButton : IAPButtonView
    {
        /*public override void OnPurchase(bool isFromStart)
        {
            base.OnPurchase(isFromStart);
            ActivateNoAds(isFromStart);
        }

        public override void Refresh()
        {
            if (_playerPrefsSaveManager.PrefsData.InAppInfoModel.NoAdsActivated)
            {
                ShowPurchasedImage();
            }
        }

        private void ActivateNoAds(bool isFromStart)
        {
            _playerPrefsSaveManager.PrefsData.InAppInfoModel.ActivateNoAds();
            if(isFromStart) return;
            SendAnalytics();
            _playerPrefsSaveManager.ForceSave();
            _signalBus.Fire<RefreshShopSignal>();
        }*/
    }
}