namespace Gameplay.Scripts.GameBootstrap
{
    public class LoadingProgressSignal
    {
        public float Value { get; private set; }

        public LoadingProgressSignal(float value)
        {
            Value = value;
        }
    }
}