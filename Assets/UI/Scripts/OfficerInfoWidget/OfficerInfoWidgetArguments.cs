namespace UI.Scripts.OfficerInfoWidget
{
    public class OfficerInfoWidgetArguments : UIArguments
    {
        public string OfficerKey { get; private set; }

        public OfficerInfoWidgetArguments(string officerKey)
        {
            OfficerKey = officerKey;
        }
    }
}