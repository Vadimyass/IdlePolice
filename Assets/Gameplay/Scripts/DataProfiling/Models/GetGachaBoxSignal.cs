using Gameplay.Configs;

public class GachaBoxGetSignal
{
    public GachaBoxType GachaBoxType { get; private set; }

public GachaBoxGetSignal(GachaBoxType gachaBoxType)
    {
        GachaBoxType = gachaBoxType;
    }
}