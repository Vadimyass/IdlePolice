using System.Collections.Generic;
using System.Linq;
using ModestTree;
using Newtonsoft.Json;
using SolidUtilities.Collections;
using Zenject;

namespace Gameplay.Scripts.DataProfiling.PrefsData
{
    public class CurrenciesModel : IPlayerPrefsData
    {
        private SignalBus _signalBus;

        public IReadOnlyList<double> BasesMoney => _basesMoney;
        public IReadOnlyList<double> BasesMoneyLifetime => _basesMoneyLifetime;
        
        private List<double> _basesMoney = new List<double>();

        private List<double> _baseListForMoney = new List<double>() { 100, 50000, 50000 };
        private List<double> _baseBasesMoneyLifetime = new List<double>() { 100, 150, 150 };
        private List<double> _basesMoneyLifetime = new ();

        [JsonIgnore]public int CrystalCount => _crystalCount;
        [JsonProperty] private int _crystalCount = 70;
        
        
        [JsonProperty] private double _donutCount = 0;
        [JsonIgnore]public double DonutCount => _donutCount;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        public void Initialize()
        {
            if (_basesMoney.IsEmpty() == true)
            {
                _basesMoney = _baseListForMoney;
            }

            if (_basesMoneyLifetime.IsEmpty() == true)
            {
                _basesMoneyLifetime = _baseBasesMoneyLifetime;
            }
        }

        public bool TrySpendMoney(double currency, int baseId)
        {
            if (_basesMoney[baseId] - currency < 0)
            {
                return false;
            }

            _basesMoney[baseId] -= currency;
            _signalBus.Fire(new ChangeCurrencySignal(CurrencyUIType.Dollar, -(int)currency));
            return true;
            
        }


        public void IncreaseCrystal(int currency)
        {
            _crystalCount += currency;
            _signalBus.Fire(new ChangeCurrencySignal(CurrencyUIType.Crystal, (int)currency));
        }
        
        public void IncreaseDonut(double currency)
        {
            _donutCount += currency;
            _signalBus.Fire(new ChangeCurrencySignal(CurrencyUIType.Crystal, (int)currency));
        }

        public bool TrySpendDonut(double currency)
        {
            if (_donutCount - currency < 0)
            {
                return false;
            }

            _donutCount -= currency;
            _signalBus.Fire(new ChangeCurrencySignal(CurrencyUIType.Donut, -(int)currency));
            return true;
        }
        
        public bool TrySpendCrystal(int currency)
        {
            if (_crystalCount - currency < 0)
            {
                return false;
            }

            _crystalCount -= currency;
            _signalBus.Fire(new ChangeCurrencySignal(CurrencyUIType.Crystal, -(int)currency));
            return true;
        }

        public void ChangeMoney(double currency, int baseId)
        {
            var newMoneyValue = _basesMoney[baseId] + currency;
            _basesMoney[baseId] =  newMoneyValue < 0 ? 0 : newMoneyValue;
            _basesMoneyLifetime[baseId] += currency;
            _signalBus.Fire(new ChangeCurrencySignal(CurrencyUIType.Dollar, (int)currency));
            
        }
    }
}