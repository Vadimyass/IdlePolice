using Gameplay.Configs;

namespace Gameplay.Scripts.DataProfiling
{
    public class ChangeCurrencySignal
    {
        public CurrencyUIType CurrencyType { get; private set; }
        public float MoneyChange { get; private set; }

        public ChangeCurrencySignal(CurrencyUIType type, float moneyChange)
        {
            CurrencyType = type;
            MoneyChange = moneyChange;
        }
    }

    public enum CurrencyUIType
    {
        Dollar,
        Crystal,
        Donut,
    }
}