using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Gameplay.Configs;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.LevelManagement;
using Gameplay.Scripts.Utils;
using UnityEngine;
using Zenject;

namespace Gameplay.Scripts.Buildings
{
    public class BaseUpgradesController
    {
        private Dictionary<BuildingName, BuildingModifiers> _buildingModifiers =
            new Dictionary<BuildingName, BuildingModifiers>();

        private List<string> _upgrades = new List<string>();

        private PlayerPrefsSaveManager _playerPrefsSaveManager;
        private BigD.Config.GameConfig _gameConfig;
        private SignalBus _signalBus;
        private LevelController _levelController;

        [Inject]
        private void Construct(PlayerPrefsSaveManager playerPrefsSaveManager, BigD.Config.GameConfig gameConfig,
            SignalBus signalBus, LevelController levelController)
        {
            _levelController = levelController;
            _signalBus = signalBus;
            _gameConfig = gameConfig;
            _playerPrefsSaveManager = playerPrefsSaveManager;
        }

        public void Init()
        {
            var baseID = _playerPrefsSaveManager.PrefsData.LevelInfoModel.CurrentLevelIndex;
            if (_playerPrefsSaveManager.PrefsData.BaseUpgradesModel.BoughtUpgrades.TryGetValue(baseID, out var list))
            {
                foreach (var upgradeKey in list)
                {
                    Debug.LogError($"Loaded upgrade: {upgradeKey}");
                    AddUpgrade(upgradeKey, false); // Load upgrades without triggering signals
                }
            }

            RefreshAllModifiers(); // Recalculate modifiers after loading upgrades
        }

        public bool HasUpgrade(string key) => _upgrades.Contains(key);

        public async void AddUpgrade(string upgradeKey, bool withSignal = true)
        {
            var upgrade = _gameConfig.UpgradeBaseConfig.GetItemByKey(upgradeKey, _playerPrefsSaveManager.PrefsData.LevelInfoModel.CurrentLevelIndex);
            _upgrades.Add(upgradeKey);

            if (withSignal)
            {
                RefreshAllModifiers();
                _signalBus.Fire(new BaseUpgradeSignal(upgrade.Key, upgrade.BuildingKey));
            }
        }

        public BuildingModifiers GetInfoForBuilding(BuildingName buildingName)
        {
            if (_buildingModifiers.TryGetValue(buildingName, out var modifiers))
            {
                return modifiers;
            }

            Debug.LogWarning($"Building {buildingName} not found in modifiers. Creating default entry...");
            var defaultModifiers = new BuildingModifiers();

            if (_buildingModifiers.TryGetValue(BuildingName.All, out var globalModifiers))
            {
                defaultModifiers.Merge(globalModifiers);
            }

            _buildingModifiers[buildingName] = defaultModifiers;
            return defaultModifiers;
        }

        private void RefreshAllModifiers()
        {
            // Clear existing modifiers
            _buildingModifiers.Clear();

            // Calculate global modifiers (BuildingName.All)
            BuildingModifiers globalModifiers = new BuildingModifiers();
            foreach (var upgradeKey in _upgrades)
            {
                var upgrade = _gameConfig.UpgradeBaseConfig.GetItemByKey(upgradeKey, _playerPrefsSaveManager.PrefsData.LevelInfoModel.CurrentLevelIndex);
                if (upgrade.BuildingKey == BuildingName.All)
                {
                    ApplyUpgradeToModifiers(upgrade, globalModifiers);
                }
            }

            // Calculate each building's modifiers
            foreach (BuildingName buildingName in Enum.GetValues(typeof(BuildingName)))
            {
                // Start with global modifiers
                BuildingModifiers buildingModifiers = new BuildingModifiers
                {
                    IncomeMultiplier = globalModifiers.IncomeMultiplier,
                    SpeedMultiplier = globalModifiers.SpeedMultiplier,
                    CrimeInterval = globalModifiers.CrimeInterval,
                    BuildingSpeedMultiplier = globalModifiers.BuildingSpeedMultiplier,
                    AllBussesSpeedMultiplier = globalModifiers.AllBussesSpeedMultiplier,
                    Crimes = globalModifiers.Crimes,
                    Capacity = globalModifiers.Capacity
                };

                // Apply building-specific upgrades
                foreach (var upgradeKey in _upgrades)
                {
                    var upgrade = _gameConfig.UpgradeBaseConfig.GetItemByKey(upgradeKey, _playerPrefsSaveManager.PrefsData.LevelInfoModel.CurrentLevelIndex);
                    if (upgrade.BuildingKey == buildingName)
                    {
                        ApplyUpgradeToModifiers(upgrade, buildingModifiers);
                    }
                }

                // Store final result
                _buildingModifiers[buildingName] = buildingModifiers;

                // Debug output for validation
                Debug.Log($"Modifiers for {buildingName}: {buildingModifiers.IncomeMultiplier}");
            }
        }

        private void ApplyUpgradeToModifiers(BaseUpgradeConfigInitializator upgrade, BuildingModifiers modifiers)
        {
            foreach (var addValue in upgrade.AdditiveValue)
            {
                switch (addValue.Key)
                {
                    case AdditiveType.Building_income_multiplier:
                        modifiers.IncomeMultiplier *= addValue.Value; // Ensure multiplicative combination
                        break;
                    case AdditiveType.Building_interaction_speed:
                        modifiers.BuildingSpeedMultiplier *= addValue.Value;
                        break;
                    case AdditiveType.Crimes:
                        modifiers.Crimes += addValue.Value;
                        break;
                    case AdditiveType.Car_speed:
                        modifiers.SpeedMultiplier *= addValue.Value;
                        break;
                    case AdditiveType.Crimes_refresh_rate:
                        modifiers.CrimeInterval = addValue.Value;
                        break;
                    case AdditiveType.All_busses_speed:
                        modifiers.AllBussesSpeedMultiplier *= addValue.Value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

    [Serializable]
    public class BuildingModifiers
    {
        public double IncomeMultiplier = 1;
        public double SpeedMultiplier = 1;
        public double CrimeInterval = 10;
        public double BuildingSpeedMultiplier = 1;
        public double AllBussesSpeedMultiplier = 1;
        public double Crimes = 2;
        public int Capacity = 2;

        // Merge another BuildingModifiers into this one
        public void Merge(BuildingModifiers other)
        {
            IncomeMultiplier *= other.IncomeMultiplier;
            SpeedMultiplier *= other.SpeedMultiplier;
            CrimeInterval = Math.Min(CrimeInterval, other.CrimeInterval);
            BuildingSpeedMultiplier *= other.BuildingSpeedMultiplier;
            AllBussesSpeedMultiplier *= other.AllBussesSpeedMultiplier;
            Crimes += other.Crimes;
            Capacity += other.Capacity;
        }
    }
}