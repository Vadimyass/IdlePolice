using System.Collections.Generic;
using Agents;
using Cysharp.Threading.Tasks;
using Gameplay.Scripts.Agents;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.LevelManagement;
using Gameplay.Scripts.Utils;
using Managers;
using Particles;
using Tutorial;
using UnityEngine;
using Zenject;

namespace Gameplay.Scripts.Buildings
{
    public class BuildingCarManager : MonoBehaviour
    {
        private Building _building;
        private AgentsManager _agentsManager;
        private List<CarAgent> _agents = new();
        private ParticleManager _particleManager;
        private int _boxCount;
        private AudioManager _audioManager;
        private SignalBus _signalBus;
        private TutorialManager _tutorialManager;
        private PlayerPrefsSaveManager _playerPrefsSaveManager;
        private LevelController _levelController;

        [Inject]
        private void Construct(AgentsManager agentsManager, LevelController levelController, PlayerPrefsSaveManager playerPrefsSaveManager, TutorialManager tutorialManager, SignalBus signalBus, AudioManager audioManager, ParticleManager particleManager)
        {
            _levelController = levelController;
            _playerPrefsSaveManager = playerPrefsSaveManager;
            _tutorialManager = tutorialManager;
            _signalBus = signalBus;
            _audioManager = audioManager;
            _particleManager = particleManager;
            _agentsManager = agentsManager;
        }
        public async UniTask Init(Building building, bool firstLaunch = false)
        {
            _building = building;

            if (firstLaunch == true)
            {
                AddBox();
                return;
            }

            var instantiateTasks = new List<UniTask>();
            for (int i = 0; i < _building.CurrentStageInfo.Cars; i++)
            {
                instantiateTasks.Add(AddAgentAsync());
            }

            await UniTask.WhenAll(instantiateTasks);
        }

        public void TryValidateSpeed()
        {
            foreach (var agent in _agents)
            {
                agent.UpdateSpeed();
            }
        }

        public async UniTask AddAgentAsync()
        {
            if (_boxCount >= 1)
            {
                _boxCount--;
            }

            var agent = await _agentsManager.AddFreeAgent(_building);
            _agents.Add(agent);
            _building.SetCarsCount();
        }
        
        
        public async void AddAgent()
        {
            if (_boxCount >= 1)
            {
                _boxCount--;
            }
            
            var agent = await _agentsManager.AddFreeAgent(_building);
            _agents.Add(agent);
            _building.SetCarsCount();

            _signalBus.Fire<CreateCarSignal>();
            
            
        }

        public void TryValidateCarsCount()
        {
            if(GetCurrentAgentsCount() >= _building.CurrentStageInfo.Cars) return;
            
            var neededCount = _building.CurrentStageInfo.Cars - GetCurrentAgentsCount() - _boxCount;

            for (int i = 0; i < neededCount; i++)
            {
                AddBox();
            }
        }

        private void AddBox()
        {
            if(_boxCount+GetCurrentAgentsCount() >= _building.CurrentStageInfo.Cars) return;
            
            var box = _agentsManager.CreateCarBox(_building);
            box.transform.rotation = _building.BuildObject.rotation;
            box.Init(AddAgent,_audioManager, _particleManager);
            _boxCount++;
        }

        public int GetCurrentAgentsCount() => _agents.Count;
    }
}