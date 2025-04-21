using Gameplay.Scripts.Tutorial;
using Zenject;

namespace Tutorial
{
    public class TutorialFactory
    {
        private readonly DiContainer _diContainer;
        private readonly TutorialConfig _tutorialConfig;

        public TutorialFactory(
            DiContainer diContainer, 
            TutorialConfig tutorialConfig
        )
        {
            _diContainer = diContainer;
            _tutorialConfig = tutorialConfig;
        }

        public ITutorial GetContextTutorial(TutorialType tutorialType)
        {
            var tutorialClassType = _tutorialConfig.GetTutorialType(tutorialType);
            return (ITutorial) _diContainer.Instantiate(tutorialClassType);
        }
    }
}