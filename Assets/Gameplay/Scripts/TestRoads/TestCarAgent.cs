using System;
using MyBox;
using Pathfinding;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Scripts.TestRoads
{
    public class TestCarAgent : MonoBehaviour
    {
        [SerializeField] private Seeker _seeker;
        [SerializeField] private AILerp _aiLerp;
        [SerializeField] private TestPointContainer _testPointContainer;

        private void Start()
        {
            _aiLerp.Teleport(transform.position);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _seeker.StartPath(transform.position, _testPointContainer.Points.GetRandom().position);
            }
        }
    }
}