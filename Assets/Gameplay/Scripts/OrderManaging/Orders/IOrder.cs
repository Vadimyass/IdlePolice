using Agents;
using Gameplay.Scripts.LevelManagement;
using UnityEngine;

namespace Gameplay.OrderManaging
{
    public interface IOrder
    {
        public OrderBlockType OrderBlockType { get; protected set; }
        public GroundType GroundType { get; protected set; }
        public void Activate(OrderRequest orderRequest, GroundType groundType);

        public void PreInteraction(CarAgent carAgent);

        public void StartInteraction();
        public void Process();

        public void OnEndInteraction();

        public void CompleteOrder();

    }
}