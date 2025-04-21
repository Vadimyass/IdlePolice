using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Scripts.TestRoads
{
    public class TestPointContainer : MonoBehaviour
    {
        [SerializeField] private List<Transform> _points;

        public List<Transform> Points => _points;
    }
}