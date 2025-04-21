using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Gameplay.Configs;
using Gameplay.OrderManaging;
using Gameplay.Scripts.Agents;
using Gameplay.Scripts.Buildings;
using Gameplay.Scripts.LockedZoneManagement;
using Gameplay.Scripts.Utils;
using MyBox;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using Random = UnityEngine.Random;

namespace Gameplay.Scripts.LevelManagement
{
    public class LevelBehaviour : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private int _level;
        [SerializeField] private List<Building> _buildings;
        [SerializeField] private PointsContainer _pointsContainer;
        [SerializeField] private LockedZoneManager _lockedZoneManager;
        
        [SerializeField] private Transform _minBorder;
        [SerializeField] private Transform _maxBorder;
        
        [SerializeField] private Transform _minBorder2;
        [SerializeField] private Transform _maxBorder2;

        [SerializeField] private double _intervalDebug;
        [SerializeField] private double _crimesCountDebug;

        public int Level => _level;

        private List<PointType> _availablePointTypes = new()
        {
            PointType.Red_1,
        };
        
        private List<GroundType> _availableGroundTypes = new()
        {
            GroundType.Road,
        };

        private List<Type> _availableOrderTypes = new()
        {
            typeof(PrisonOrder)
        };

        private CameraController _cameraController;
        private BaseUpgradesController _baseUpgradesController;
        private OrderManager _orderManager;

        public List<PointType> AvailablePointTypes => _availablePointTypes;
        public List<GroundType> AvailableGroundTypes => _availableGroundTypes;
        public List<Type> AvailableOrderTypes => _availableOrderTypes;
        
        public PointsContainer PointsContainer => _pointsContainer;
        public BaseUpgradesController BaseUpgradesController => _baseUpgradesController;

        [Inject]
        private void Construct(CameraController cameraController, BaseUpgradesController baseUpgradesController,OrderManager orderManager)
        {
            _orderManager = orderManager;
            _baseUpgradesController = baseUpgradesController;
            _cameraController = cameraController;
        }
        public async UniTask Init()
        {
            foreach (var building in _buildings)
            {
                building.Init();
            }
            
            _lockedZoneManager.Init();
            _pointsContainer.Init(10);
            _cameraController.SetBordersForDragCamera(_maxBorder.position,_minBorder.position, _maxBorder2.position, _minBorder2.position);
            await UniTask.DelayFrame(2);
        }

        public int GetRandomIndexOfOpenedAreas()
        {
            return Random.Range(0, _lockedZoneManager.GetCountOfOpenZones());
        }
        

        [ButtonMethod]
        public void SetInterval()
        {
            _orderManager.SetOrderInterval(_intervalDebug);
        }
        
        [ButtonMethod]
        public void SetCrimesCount()
        {
            _orderManager.SetCrimesCount(_crimesCountDebug);
        }
        

        public void AddAvailablePointType(PointType type)
        {
            if(_availablePointTypes.Contains(type)) return;
            
            BigDDebugger.LogError("add", type.ToString());
            _availablePointTypes.Add(type);
        }
        
        public void AddAvailableGroundType(GroundType type)
        {
            if(_availableGroundTypes.Contains(type)) return;
            
            BigDDebugger.LogError("add", type.ToString());
            _availableGroundTypes.Add(type);
        }

        public void SetAvailableOrderType(Type type)
        {
            if(_availableOrderTypes.Contains(type) || type is null) return;
            _availableOrderTypes.Clear();
            
            _availableOrderTypes.Add(type);
        }
        public Building GetBuildingByType(BuildingName buildingName)
        {
            return _buildings.FirstOrDefault(x => x.BuildingName == buildingName);
        }
        
        public Building GetBuildingByKey(string key)
        {
            return _buildings.FirstOrDefault(x => x.BuildingKey == key);
        }

        public Building GetBuildingByAgentType(AgentType agentType)
        {
            for (int i = 0; i < _buildings.Count; i++)
            {
                var building = _buildings[i];
                if (building.CarAgentType == agentType)
                {
                    return building;
                } 
            }
            Debug.LogError($"Building with car type {agentType} doesn`t exist!!");
            return null;
        }

        public List<Building> GetBuiltBuildingByKey(string key)
        {
            var list = new List<Building>();

            foreach (var building in _buildings)
            {
                if (building.IsBuilt && building.BuildingKey == key)
                {
                    list.Add(building);
                }
            }

            return list;
        }

        public List<Building> GetAllBuiltBuildings()
        {
            var list = new List<Building>();

            foreach (var building in _buildings)
            {
                if (building.IsBuilt)
                {
                    list.Add(building);
                }
            }

            return list;
        }
        
        public bool TryGetBuildingByAssignedOfficer(string officerKey, out Building building)
        {
            building = null;

            foreach (var building1 in _buildings)
            {
                if(building1.OfficerSet == officerKey)
                {
                    building = building1;
                    return true;
                }
            }
            
            return false;
        }

        public bool IsAllDistrictsOpen()
        {
           return _lockedZoneManager.IsAllZonesOpen();
        }
        
        public bool IsAllBuildingsMaxLevel()
        {
            return _buildings.All(x => x.IsMaxLevel() == true);
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            foreach (var building in _buildings)
            {
                building.OnClickOutside();
            }
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            var pointA = _minBorder.position;
            var pointB = _maxBorder.position;
            Vector3 topLeft = new Vector3(pointA.x, 150, pointA.z);
            Vector3 topRight = new Vector3(pointB.x, 150, pointA.z);
            Vector3 bottomLeft = new Vector3(pointA.x, 150, pointB.z);
            Vector3 bottomRight = new Vector3(pointB.x,150, pointB.z);

            // Рисуем линии по периметру
            Gizmos.DrawLine(topLeft, topRight);     // Верхняя линия
            Gizmos.DrawLine(topRight, bottomRight); // Правая линия
            Gizmos.DrawLine(bottomRight, bottomLeft); // Нижняя линия
            Gizmos.DrawLine(bottomLeft, topLeft); 
            
            Gizmos.color = Color.blue;
            pointA = _minBorder2.position;
            pointB = _maxBorder2.position;
            Vector3 topLeft2 = new Vector3(pointA.x, 150, pointA.z);
            Vector3 topRight2 = new Vector3(pointB.x, 150, pointA.z);
            Vector3 bottomLeft2 = new Vector3(pointA.x, 150, pointB.z);
            Vector3 bottomRight2 = new Vector3(pointB.x, 150, pointB.z);

            // Рисуем линии по периметру
            Gizmos.DrawLine(topLeft2, topRight2);     // Верхняя линия
            Gizmos.DrawLine(topRight2, bottomRight2); // Правая линия
            Gizmos.DrawLine(bottomRight2, bottomLeft2); // Нижняя линия
            Gizmos.DrawLine(bottomLeft2, topLeft2); 
            
            Gizmos.DrawLine(topLeft2, topLeft);     
            Gizmos.DrawLine(topRight2, topRight); 
            Gizmos.DrawLine(bottomRight2, bottomRight); 
            Gizmos.DrawLine(bottomLeft2, bottomLeft); 
        }
        
        public int GetCarCount()
        {
            int count = 0;
            foreach (var building in _buildings)
            {
                if (building != null)
                {
                    count += building.GetCarsCount();
                }
            }

            return count;
        }
    }
}