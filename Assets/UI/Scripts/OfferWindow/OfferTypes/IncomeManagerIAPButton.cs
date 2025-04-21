using System;
using Gameplay.Scripts.Buildings;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.Scripts.IAPController
{
    public class IncomeManagerIAPButton : IAPButtonView
    {
        /*[SerializeField] private int _incomeAmount;


        public override void OnPurchase(bool isFromStart)
        {
            base.OnPurchase(isFromStart);
            AddHourManager(isFromStart);
        }

        public override void Refresh()
        {
            if (_incomeAmount == 50 && _playerPrefsSaveManager.PrefsData.InAppInfoModel.Extra50IncomeActivated)
            {
                ShowPurchasedImage();
            }

            if (_incomeAmount == 100 && _playerPrefsSaveManager.PrefsData.InAppInfoModel.Extra100IncomeActivated)
            {
                ShowPurchasedImage();
            }
        }


        private void AddHourManager(bool isFromStart)
        {
            if (_incomeAmount == 50)
            {
                _playerPrefsSaveManager.PrefsData.InAppInfoModel.Activate50ExtraIncome();
            }

            if (_incomeAmount == 100)
            {
                _playerPrefsSaveManager.PrefsData.InAppInfoModel.Activate100ExtraIncome();
            }

            if(isFromStart) return;
            SendAnalytics();
            _signalBus.Fire<RefreshShopSignal>();
            _playerPrefsSaveManager.ForceSave();
        }*/
    }
}