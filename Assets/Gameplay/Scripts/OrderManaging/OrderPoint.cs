using UnityEngine;

namespace Gameplay.OrderManaging
{
    public class OrderPoint : MonoBehaviour
    {
        [SerializeField] private PointType _pointType;
        [SerializeField] private GroundType _groundType;

        public PointType PointType => _pointType;
        public GroundType GroundType => _groundType;

        private bool _isOccupied;
        public bool IsOccupied => _isOccupied;

        public void SetOccupied(bool isOccupied)
        {
            _isOccupied = isOccupied;
        }
    }

    public enum GroundType
    {
        Road,
        Water
    }
}