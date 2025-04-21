using BigD.Config;
using Gameplay.Scripts.LevelManagement;

namespace Gameplay.OrderManaging
{
    public class DefaultOrderRequest : OrderRequest
    {
        public LevelController LevelController;
        public OrderPoint OrderPoint;
        public OrderManager OrderManager;
        public GameConfig GameConfig;
    }
}