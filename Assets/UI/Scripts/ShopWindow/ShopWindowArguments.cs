using UI.Scripts.OfferWindow;

namespace UI.Scripts.ShopWindow
{
    public class ShopWindowArguments : UIArguments
    {
        private OfferType _offerType;
        public OfferType OfferType => _offerType;

        public ShopWindowArguments(OfferType offerType = OfferType.ForBeginners)
        {
            _offerType = offerType;
        }
    }
}