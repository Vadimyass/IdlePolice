using System;
using System.Threading;
using Agents;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Gameplay.OrderManaging
{
    public class PatrolOrderRunner : OrderRunner
    {
        private OrderManager _orderManager;

        [Inject]
        private void Construct(OrderManager orderManager)
        {
            _orderManager = orderManager;   
        }
        public override void Init(CarAgent carAgent)
        {
            base.Init(carAgent);
            _orderManager.AddSeeker(this, carAgent.GroundType);
        }


        public override void CompleteOrder()
        {
            _orderManager.AddSeeker(this, _carAgent.GroundType);
            Dispose();
        }

    }
}