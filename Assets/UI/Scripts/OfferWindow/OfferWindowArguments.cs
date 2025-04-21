namespace UI.Scripts.OfferWindow
{
    public class OfferWindowArguments : UIArguments
    {
        private OfferType _offerType;
        private bool _withNextOnDecline;
        public OfferType OfferType => _offerType;
        public bool WithNextOnDecline => _withNextOnDecline;

        public OfferWindowArguments(OfferType offerType, bool withNextOnDecline = false)
        {
            _withNextOnDecline = withNextOnDecline;
            _offerType = offerType;
        }
    }
}