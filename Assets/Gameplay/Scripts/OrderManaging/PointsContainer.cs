using System;
using System.Collections.Generic;
using System.Linq;
using Agents;
using DG.DemiLib;
using Gameplay.Huds.Scripts;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.LevelManagement;
using Gameplay.Scripts.Utils;
using MyBox;
using Tutorial;
using UnityEngine;
using Zenject;

namespace Gameplay.OrderManaging
{
    public class PointsContainer : MonoBehaviour
    {
        [SerializeField] private List<PointsForZone> _points;
        [SerializeField] private HudPool _hudPool;
        private LevelController _levelController;
        private PlayerPrefsSaveManager _prefsSaveManager;


        public void Init(int count)
        {
            _hudPool.Init(count);
        }

        [Inject]
        private void Construct(LevelController levelController, PlayerPrefsSaveManager prefsSaveManager, OrderManager orderManager)
        {
            _prefsSaveManager = prefsSaveManager;
            _levelController = levelController;
        }

        public OrderHud GetHud(IOrder order)
        {
            var orderHud = _hudPool.GetOrderHud();
            orderHud.Init(order);
            return orderHud;
        }

        public void ReturnHud(OrderHud orderHud)
        {
            _hudPool.ReturnHud(orderHud);
        }

        public OrderPoint GetRandomPoint(int cityArea, GroundType groundType)
        {
            if (_prefsSaveManager.PrefsData.TutorialModel.IsContextTutorDone(TutorialType.WelcomeTutorial) == false)
            {
                return _points[0].Points[0];
            }
            
            var points = new List<OrderPoint>();
            for (int i = 0; i <= cityArea; i++)
            {
                points.AddRange(_points[i].Points.Where(x => _levelController.CurrentLevel.AvailablePointTypes.Contains(x.PointType) && x.GroundType == groundType && x.IsOccupied == false).ToList());
            }

            if (points.Count == 0)
            {
                return null;
            }
            
            return points.GetRandom();
        }
    }
 
    [Serializable]
    public struct PointsForZone
    {
        public List<OrderPoint> Points;
    }
}