using Gameplay.Scripts.Tutorial;
using Managers;
using Tutorial;
using UnityEngine;
using Zenject;

namespace Installers.Scripts
{
    public class TutorialInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private TutorialManager _tutorialManagerPrefab = default;
        [SerializeField] private TutorialContextObjectConfig _tutorialContextObjectConfig = default;
        [SerializeField] private TutorialConfig _tutorialConfig = default;
        [SerializeField] private bool _isTutorialActive = true;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<TutorialManager>().FromComponentInNewPrefab(_tutorialManagerPrefab).AsSingle()
                .WithArguments(_tutorialConfig, _tutorialContextObjectConfig, _isTutorialActive).NonLazy();
            Container.Bind<TutorialStepConditionController>().FromNew().AsSingle().NonLazy();
        }
    }
}