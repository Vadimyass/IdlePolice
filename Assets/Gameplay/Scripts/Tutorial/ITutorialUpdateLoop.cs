namespace Gameplay.Scripts.Tutorial
{
    public interface ITutorialUpdateLoop
    {
        void AddTutorialObserver(ITutorialUpdateObserver updateObserver);
        void RemoveTutorialObserver(ITutorialUpdateObserver updateObserver);
    }
}