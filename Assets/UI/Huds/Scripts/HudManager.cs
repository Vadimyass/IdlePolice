using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay.Configs;
using Gameplay.Scripts.Buildings;
using Gameplay.Scripts.Loaders;
using Gameplay.Scripts.TimeManagement;
using Gameplay.Scripts.Utils;
using Pool;
using SolidUtilities.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace UI.Huds.Scripts
{
    public class HudManager : MonoBehaviour
    {
        [SerializeField] private AssetReference _buildingHudPool;
        [SerializeField] private AssetReference _dispatchCenterHudPool;
        private Dictionary<IHudOwner,HudContainer> _fortAgents = new ();
        private DiContainer _container;
        private IResourceLoader _resourceLoader;

        [Inject]
        private void Construct(DiContainer container,IResourceLoader resourceLoader)
        {
            _resourceLoader = resourceLoader;
            _container = container;
        }

        public void SetOffsetForHud(IHudOwner hudOwner,Vector3 offset)
        {
            _fortAgents[hudOwner].SetOffset(offset);
        }

        public async UniTask TryCreateAgentHud(IHudOwner hudOwner)
        {
            if (_fortAgents.ContainsKey(hudOwner))
            {
                return;
            }
            
            GameObject hudAsset = null;
            
            switch (hudOwner)
            {
                case Building building:
                    if (building.BuildingName == BuildingName.Dispatch_center)
                    {
                        hudAsset = await _resourceLoader.InstantiateAsync(_dispatchCenterHudPool, transform);
                        break;
                    }
                    hudAsset = await _resourceLoader.InstantiateAsync(_buildingHudPool,transform);
                    break;
            }

            if (hudAsset == null)
            {
                BigDDebugger.LogError(GetType(),"please add interface specifications this!");
                return;
            }
            
            var newHud = hudAsset.GetComponent<HudContainer>();
            _container.InjectGameObject(newHud.gameObject);
            newHud.Init(hudOwner);

            _fortAgents.Add(hudOwner,newHud);
            await UniTask.CompletedTask;
        }
    }
}