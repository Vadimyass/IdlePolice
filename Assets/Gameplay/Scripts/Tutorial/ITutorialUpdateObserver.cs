namespace Gameplay.Scripts.Tutorial
{
    public interface ITutorialUpdateObserver
    {
        /// <summary>
        /// Updating method
        /// </summary>
        /// <returns>false if shoud stop observe</returns>
        bool TryUpdate();
    }
}