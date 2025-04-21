using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.OrderManaging;
using Gameplay.Scripts.MeshBuilder;
using Gameplay.Scripts.Tools;
using Gameplay.Scripts.Utils;
using Gameplay.Scripts.Agents;
using Gameplay.Scripts.Buildings;
using Gameplay.Scripts.LevelManagement;
using Gameplay.Scripts.Locker;
using MyBox;
using Particles;
using Pathfinding;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Agents
{
    public class CarAgent : AILerp
    {
        [SerializeField] protected Seeker _navMeshAgent;
        [SerializeField] protected CriminalCarHud _criminalHud;
        [SerializeField] protected float _distance;

        [SerializeField] protected AgentType _agentType;
        //[SerializeField] private AILerp _aiLerp;
        [SerializeField] protected GroundType _groundType;
        
        [SerializeField] protected ParticleSystem _particleSystem;
        [SerializeField] protected AgentAnimation _animation;
        [SerializeField] protected ParticleSystem _lightsParticle;
        public IOrderRunner OrderRunner { get; private set; }
        private OrderManager _orderManager;

        private CustomLineRenderer _lineRenderer; 
        private List<Vector3> _pathPoints;
        private int _currentPointIndex;
        private MeshBuilderManager _meshBuilderManager;
        private int _pointsToCheck = 3;
        private bool _isIdle;

        private bool _isReachedDestination;

        protected double BaseSpeed = 3;
        private ParticleManager _particleManager;
        private ParticleSystem _sleepParticle;
        private LevelController _levelController;
        private CancellationTokenSource _cancellationTokenSource;
        private OrderPoint _idlePoint;
        private bool _isUpdatingTrail;

        private LockController _lockController;

        private Vector3 _targetPosition;

        public AgentType AgentType => _agentType;
        public Building LinkedBuilding { get; protected set; }
        public Transform _target { get; set; }
        public CriminalCarHud CriminalHud => _criminalHud;
        public GroundType GroundType => _groundType;

        [Inject]
        private void Construct(OrderManager orderManager, LockController lockController, MeshBuilderManager meshBuilderManager,ParticleManager particleManager,LevelController levelController)
        {
            _lockController = lockController;
            _levelController = levelController;
            _particleManager = particleManager;
            _meshBuilderManager = meshBuilderManager;
            _orderManager = orderManager;
        }

        public async UniTask Init(IOrderRunner orderRunner, Building building)
        {
            OrderRunner = orderRunner;
            OrderRunner.Init(this);
            _lineRenderer = await _meshBuilderManager.CreateLineRenderer();
            LinkedBuilding = building;
            _lineRenderer.SetPoints(new List<Vector3>());
            speed = 0;
            _currentPointIndex = 0;
            
            UpdateSpeed();
            
            await UniTask.CompletedTask;
            
            if (AgentType != AgentType.Patrol || AgentType != AgentType.BoatPatrol) return;

            StartIdle();
        }

        public async void StartIdle()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _lightsParticle.Stop();
            
            if (_lockController.HaveLock<OrderSpawnLocker>())
            {
                return;
            }
            
            var isCanceled = await UniTask.Delay(TimeSpan.FromSeconds(1)).AttachExternalCancellation(_cancellationTokenSource.Token).SuppressCancellationThrow();
            if (isCanceled)
            {
                return;
            }
            UpdateSpeed();
            _isIdle = true;
            _idlePoint = _levelController.CurrentLevel.PointsContainer.GetRandomPoint(_levelController.CurrentLevel.GetRandomIndexOfOpenedAreas(), _groundType);

            if (_idlePoint != null)
            {

                _idlePoint.SetOccupied(true);
                isCanceled = await SetDestination(_idlePoint.transform.position)
                    .AttachExternalCancellation(_cancellationTokenSource.Token).SuppressCancellationThrow();

                if (isCanceled)
                {
                    return;
                }
            }

            _sleepParticle = _particleManager.PlayParticleInTransformWithOffset(ParticleType.sleep, transform,Quaternion.identity,Vector3.up * 4);
        }

        public void StopIdle()
        {
            _idlePoint?.SetOccupied(false);
            _cancellationTokenSource?.Cancel();
            _isIdle = false;
            _particleManager.ReturnParticle(_sleepParticle);
            _lightsParticle.Play();
        }
        

        public virtual void UpdateSpeed()
        {
            speed = (float) (BaseSpeed * LinkedBuilding.BuildingModifiers.SpeedMultiplier);
        }

        public void Warp(Vector3 position)
        {
            Teleport(position);
        }

        private async UniTaskVoid UpdateTrailRoutine()
        {
            if (_isIdle) return;
    
            while (true)
            {
                if (_pathPoints == null || _pathPoints.Count == 0)
                {
                    await UniTask.DelayFrame(1);
                    continue;
                }

                int endIndex = Mathf.Min(_currentPointIndex + _pointsToCheck, _pathPoints.Count);
                for (int i = _currentPointIndex; i < endIndex; i++)
                {
                    Vector3 carPosFlat = new Vector3(position.x, 0, position.z);
                    Vector3 pointPosFlat = new Vector3(_pathPoints[i].x, 0, _pathPoints[i].z);

                    if ((carPosFlat - pointPosFlat).sqrMagnitude <= _distance * _distance)
                    {
                        _currentPointIndex = i + 1;
                    }
                }

                UpdateTrail();

                await UniTask.Delay(TimeSpan.FromMilliseconds(CalculateDelayTrail())); // Update every 100ms
            }
        }
        
        private float CalculateDelayTrail()
        {
            float defaultSpeed = 3f;
            float baseValue = 80f;

            return speed == 0 ? baseValue : baseValue * (defaultSpeed / speed);
        }

        protected override async void OnPathComplete(Path pathReceipt)
        {
            base.OnPathComplete(pathReceipt);
            if (pathReceipt.error || _isIdle) return;
            
            _pathPoints = pathReceipt.vectorPath.ToList();
            
            int checkPoints = Mathf.Min(1, _pathPoints.Count); // Проверяем последние 3 точки (или меньше, если их недостаточно)
            bool shouldReplace = true;

            for (int i = 1; i <= checkPoints; i++)
            {
                if ((_pathPoints[^i] - _targetPosition).sqrMagnitude < 0.01f) // Проверяем совпадение
                {
                    shouldReplace = false;
                    break;
                }
            }

            if (shouldReplace)
            {
                _pathPoints.RemoveRange(_pathPoints.Count - checkPoints, checkPoints); // Удаляем последние точки
                _pathPoints.Add(_targetPosition);
            }
            
            _pathPoints = PathUtilities.AddDetailToPath(_pathPoints, 4);
            

            

            _lineRenderer.SetPoints(_pathPoints);
            _lineRenderer.Show();
            _currentPointIndex = 0;

            if (!_isUpdatingTrail)
            {
                _isUpdatingTrail = true;
                UpdateTrailRoutine().Forget();
            }

            await UniTask.Delay(TimeSpan.FromSeconds(0.3f));
            UpdateSpeed();
        }

        private void UpdateTrail()
        {
            if (_currentPointIndex >= _pathPoints.Count) return;

            int pointsToShow = _pathPoints.Count - _currentPointIndex;
            
            // Only update if significant change occurred
            if (pointsToShow % 2 == 0) // Every 3rd point
            {
                _lineRenderer.SetPoints(_pathPoints.GetRange(_currentPointIndex, pointsToShow));
            }
        }

        public override void OnTargetReached()
        {
            _lineRenderer.Hide();
            _isReachedDestination = true;
            speed = 0;
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public async UniTask SetDestination(Vector3 position)
        {
            if (_particleSystem != null)
            {
                _particleSystem.Play();
            }

            _isReachedDestination = false;
            _animation.PlayMoveAnimation();
            _targetPosition = position;
            _navMeshAgent.StartPath(transform.position, position);
            await UniTask.WaitUntil(() => _isReachedDestination);

            if (_particleSystem != null)
            {
                _particleSystem.Stop();
            }
            _animation.StopMoveAnimation();
        }

        public void ResetPath()
        {
            ClearPath();
            OnTargetReached();
            _lineRenderer.ClearMesh();
        }

        public void StartFightAnimation()
        {
            _animation.PlayShakeAnimation();
        }
        
        public void StopFightAnimation()
        {
            _animation.StopShakeAnimation();
        }

        public void ShowCriminalHud()
        {
            var seq = DOTween.Sequence();
            
            _criminalHud.ShowImage();
            
            seq.Append(_criminalHud.transform.DOScale(0.2f, 0.2f).From(0));
            seq.Join(_criminalHud.transform.DOLocalRotate(Vector3.zero, 0.2f).From(Vector3.right * 40 + Vector3.forward * 40));
            
            _criminalHud.transform.parent.gameObject.SetActive(true);
        }

        public void HideCriminalHud()
        {
            var seq = DOTween.Sequence();
            
            seq.Append(_criminalHud.transform.DOScale(0, 0.2f).From(0.2f));
            seq.Join(_criminalHud.transform.DOLocalRotate(Vector3.right * 40 + Vector3.forward * 40, 0.2f).From(Vector3.zero));
            
            _criminalHud.transform.parent.gameObject.SetActive(false);
        }
    }
}
