using Gameplay.Scripts;
using Pointers;
using UI;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "UIInstaller", menuName = "Installers/UIInstaller")]
public class UIInstaller : ScriptableObjectInstaller<UIInstaller>
{
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private UIConfig _uiConfig;
    [SerializeField] private SpritesConfig _spritesConfig;
    [SerializeField] private SpritesConfigDictionaries _spritesConfigDictionaries;
    [SerializeField] private PointerManager _pointerManager;

    public override void InstallBindings()
    {
        Container
             .Bind<InputController>()
             .FromNew()
             .AsSingle()
             .NonLazy();
        Container
            .Bind<SpritesConfig>()
            .FromInstance(_spritesConfig)
            .AsSingle()
            .NonLazy();
        Container
            .Bind<SpritesConfigDictionaries>()
            .FromInstance(_spritesConfigDictionaries)
            .AsSingle()
            .NonLazy();
         Container
             .BindInterfacesAndSelfTo<UIManager>()
             .FromComponentInNewPrefab(_uiManager)
             .AsSingle()
             .NonLazy();
         Container
             .BindInterfacesAndSelfTo<PointerManager>()
             .FromComponentInNewPrefab(_pointerManager)
             .AsSingle()
             .NonLazy();
         Container
            .Bind<UIFactory>()
            .FromNew()
            .AsTransient()
            .WithArguments(_uiConfig);
    }
}