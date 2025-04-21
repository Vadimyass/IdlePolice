using System.Collections.Generic;

namespace Gameplay.Scripts.Tutorial
{
    public interface ITutorial
    {
        void Configurate();
        List<TutorialStepBase> GetSteps();
    }
}