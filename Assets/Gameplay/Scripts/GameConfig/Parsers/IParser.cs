namespace Gameplay.Configs
{
    public interface IParser
    {
        object ParseObject(string csvString);
    }
}