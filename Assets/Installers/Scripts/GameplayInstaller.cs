using Gameplay.Scripts._3DTextPoolManagement;
using Gameplay.Scripts.MeshBuilder;
using UnityEngine;
using Zenject;

namespace Installers.Scripts
{
    public class GameplayInstaller : ScriptableObjectInstaller<GameplayInstaller>
    {
        [SerializeField] private MeshBuilderManager _meshBuilderManager;
        [SerializeField] private TextPoolManager _textPoolManager;
        public override void InstallBindings()
        {
            Container.Bind<MeshBuilderManager>().FromComponentInNewPrefab(_meshBuilderManager).AsSingle().NonLazy();
            Container.Bind<TextPoolManager>().FromComponentInNewPrefab(_textPoolManager).AsSingle().NonLazy();
        }
    }
}