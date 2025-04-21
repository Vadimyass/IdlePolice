using Zenject;

public class OfficerGetSignal
{
    public string Key { get; private set;}
    
    public OfficerGetSignal(string key)
    {
        Key = key;
    }
}