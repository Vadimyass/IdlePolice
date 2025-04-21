using Agents;

namespace Gameplay.OrderManaging
{
    public interface IOrderRunner
    {
        void Init(CarAgent carAgent);
        void ActivateOrder(IOrder order);

        void CompleteOrder();

    }
}