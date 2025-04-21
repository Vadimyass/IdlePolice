using Gameplay.Scripts.Buildings;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.Signals;
using Gameplay.Scripts.Utils;
using UI.Scripts.OfferWindow;
using UnityEngine;
using Zenject;

namespace Gameplay.Scripts.IAPController
{
    public class OfferIAPButton : IAPButtonView
    {
        /*[SerializeField] private int _incomeAmount;
        [SerializeField] private int _hoursAmount;
        
        [SerializeField] private CurrencyType _currencyType;
        [SerializeField] private int _currencyAmount;
        [SerializeField] private bool _noAds;
        [SerializeField] private OfferType _offerType;
        

        public override void Refresh()
        {
            if (_playerPrefsSaveManager.PrefsData.InAppInfoModel.IsOfferActivated(_offerType) || _playerPrefsSaveManager.PrefsData.InAppInfoModel.IsOfferActivated(OfferType.ForPatriots))
            {
                ShowPurchasedImage();
            }
        }

        public override void OnPurchase(bool isFromStart)
        {
            base.OnPurchase(isFromStart);
            BuyOffer(isFromStart);
        }

        private void BuyOffer(bool isFromStart)
        {
            if (isFromStart == false)
            {
                switch (_currencyType)
                {
                    case CurrencyType.Dollar:
                        _playerPrefsSaveManager.PrefsData.CurrenciesInfoModel.ChangeMoney(_currencyAmount,
                            _militaryBaseController.CurrentBase.ID);
                        break;
                    case CurrencyType.Crystal:
                        _playerPrefsSaveManager.PrefsData.CurrenciesInfoModel.IncreaseCrystal(_currencyAmount);
                        break;
                }
            }


            if (_noAds)
            {
                _playerPrefsSaveManager.PrefsData.InAppInfoModel.ActivateNoAds();
            }

            if (_hoursAmount == 1 &&
                _playerPrefsSaveManager.PrefsData.InAppInfoModel.ExtraTime1BackgroundActivated == false)
            {
                _playerPrefsSaveManager.PrefsData.InAppInfoModel.Activate1ExtraTimeBackground();
            }

            if (_hoursAmount == 3 &&
                _playerPrefsSaveManager.PrefsData.InAppInfoModel.ExtraTime3BackgroundActivated == false)
            {
                _playerPrefsSaveManager.PrefsData.InAppInfoModel.Activate3ExtraTimeBackground();
            }

            if (_hoursAmount == 23)
            {
                _playerPrefsSaveManager.PrefsData.InAppInfoModel.Activate1ExtraTimeBackground();
                _playerPrefsSaveManager.PrefsData.InAppInfoModel.Activate3ExtraTimeBackground();
                _playerPrefsSaveManager.PrefsData.InAppInfoModel.Activate7ExtraTimeBackground();
                _playerPrefsSaveManager.PrefsData.InAppInfoModel.Activate12ExtraTimeBackground();
            }


            if (_incomeAmount == 50)
            {
                _playerPrefsSaveManager.PrefsData.InAppInfoModel.Activate50ExtraIncome();
                _incomeController.Refresh();
            }

            if (_incomeAmount == 100)
            {
                _playerPrefsSaveManager.PrefsData.InAppInfoModel.Activate100ExtraIncome();
                _incomeController.Refresh();
            }

            if (_incomeAmount == 150)
            {
                _playerPrefsSaveManager.PrefsData.InAppInfoModel.Activate50ExtraIncome();
                _playerPrefsSaveManager.PrefsData.InAppInfoModel.Activate100ExtraIncome();
                _incomeController.Refresh();
            }

            _playerPrefsSaveManager.PrefsData.InAppInfoModel.ActivateOffer(_offerType);
            if (isFromStart) return;

            SendAnalytics();
            _signalBus.Fire<RefreshShopSignal>();

            _playerPrefsSaveManager.ForceSave();

        }*/
    }
}