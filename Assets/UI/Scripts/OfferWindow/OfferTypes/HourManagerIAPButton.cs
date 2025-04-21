using Gameplay.Scripts.Buildings;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.Signals;
using Gameplay.Scripts.Utils;
using UI.Scripts.ShopWindow;
using UnityEngine;
using Zenject;

namespace Gameplay.Scripts.IAPController
{
    public class HourManagerIAPButton : IAPButtonView
    {
        /*[SerializeField] private int _hoursAmount;
        
        
        public override void OnPurchase(bool isFromStart)
        {
            base.OnPurchase(isFromStart);
            AddHourManager(isFromStart);
        }

        public override void Refresh()
        {
            /*if(_hoursAmount == 1 && _playerPrefsSaveManager.PrefsData.InAppInfoModel.ExtraTime1BackgroundActivated)
            {
                ShowPurchasedImage();
            }
            
            if(_hoursAmount == 3 && _playerPrefsSaveManager.PrefsData.InAppInfoModel.ExtraTime3BackgroundActivated)
            {
                ShowPurchasedImage();
            }
            
            if(_hoursAmount == 7 && _playerPrefsSaveManager.PrefsData.InAppInfoModel.ExtraTime7BackgroundActivated)
            {
                ShowPurchasedImage();
            }
            
            if(_hoursAmount == 12 && _playerPrefsSaveManager.PrefsData.InAppInfoModel.ExtraTime12BackgroundActivated)
            {
                ShowPurchasedImage();
            }#1#
        }


        private void AddHourManager(bool isFromStart)
        {
            switch (_hoursAmount)
            {
                /*case 1:
                    _playerPrefsSaveManager.PrefsData.InAppInfoModel.Activate1ExtraTimeBackground();
                    break;
                case 3:
                    _playerPrefsSaveManager.PrefsData.InAppInfoModel.Activate3ExtraTimeBackground();
                    break;
                case 7:
                    _playerPrefsSaveManager.PrefsData.InAppInfoModel.Activate7ExtraTimeBackground();
                    break;
                case 12:
                    _playerPrefsSaveManager.PrefsData.InAppInfoModel.Activate12ExtraTimeBackground();
                    break;#1#
            }

            if(isFromStart) return;
            SendAnalytics();
            _signalBus.Fire<RefreshShopSignal>();
            _playerPrefsSaveManager.ForceSave();
        }*/
    }
}