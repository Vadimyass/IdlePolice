using Agents;
using Gameplay.Scripts.LevelManagement;
using Gameplay.Scripts.Utils;
using UnityEngine;

namespace Gameplay.OrderManaging
{
    public class Order : IOrder
    {
        protected OrderBlockType _orderBlockType = OrderBlockType.NonActivated;
        private GroundType _groundType;

        OrderBlockType IOrder.OrderBlockType
        {
            get => _orderBlockType;
            set => _orderBlockType = value;
        }

        GroundType IOrder.GroundType
        {
            get => _groundType;
            set => _groundType = value;
        }

        public virtual void Activate(OrderRequest orderRequest, GroundType groundType)
        {
            _orderBlockType = OrderBlockType.Activated;
            _groundType = groundType;
        }

        public virtual void PreInteraction(CarAgent carAgent)
        {
            _orderBlockType = OrderBlockType.PreInteraction;
        }

        public virtual void StartInteraction()
        {
            _orderBlockType = OrderBlockType.StartInteraction;
            
        }

        public virtual void Process()
        {
            _orderBlockType = OrderBlockType.Process;
        }

        public virtual void OnEndInteraction()
        {
            _orderBlockType = OrderBlockType.EndInteraction;
        }

        public virtual void CompleteOrder()
        {
            
        }

        protected void MoveToNextStep()
        {
            _orderBlockType = _orderBlockType.Next<OrderBlockType>();
        }
    }
}