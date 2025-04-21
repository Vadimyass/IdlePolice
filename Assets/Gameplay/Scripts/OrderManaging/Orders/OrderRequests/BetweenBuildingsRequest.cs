using System;
using Gameplay.Scripts.Buildings;

namespace Gameplay.OrderManaging
{
    public class BetweenBuildingsRequest : OrderRequest
    {
        public readonly Building PreviousBuilding;
        public readonly Building NextBuilding;
        public readonly int Capacity;
        

        public BetweenBuildingsRequest(Building previousBuilding,Building nextBuilding, int capacityOrder)
        {
            PreviousBuilding = previousBuilding;
            NextBuilding = nextBuilding;
            Capacity = capacityOrder;
        }
    }
}