using System.Collections.Generic;
using Gameplay.Scripts.Buildings;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.Utils;
using UnityEngine;
using Zenject;

namespace Gameplay.Scripts.IAPController
{
    public class CurrencyIAPButton : IAPButtonView
    {
        /*[SerializeField] private CurrencyUIType _currencyType;
        [SerializeField] private int _currencyAmount;

        public override void OnPurchase(bool isFromStart)
        {
            base.OnPurchase(isFromStart);
            AddCurrency(isFromStart);
        }

        private void AddCurrency(bool isFromStart)
        {
            if(isFromStart) return;
            
            switch (_currencyType)
            {
                case CurrencyUIType.Dollar:
                    _playerPrefsSaveManager.PrefsData.CurrenciesModel.ChangeMoney(_currencyAmount,_militaryBaseController.CurrentBase.ID);
                    break;
                case CurrencyUIType.Crystal:
                    _playerPrefsSaveManager.PrefsData.CurrenciesModel.IncreaseCrystal(_currencyAmount);
                    break;
            }
            
            SendAnalytics();
            _playerPrefsSaveManager.ForceSave();
        }*/
    }
}