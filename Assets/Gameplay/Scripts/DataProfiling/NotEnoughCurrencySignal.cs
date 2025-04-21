using Gameplay.Configs;

namespace Gameplay.Scripts.DataProfiling
{
    public class NotEnoughCurrencySignal
    {
        public CurrencyUIType CurrencyType { get; private set; }

        public NotEnoughCurrencySignal(CurrencyUIType type)
        {
            CurrencyType = type;
        }
    }
}