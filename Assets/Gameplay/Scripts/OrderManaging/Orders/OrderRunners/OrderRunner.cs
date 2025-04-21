using System;
using System.Threading;
using Agents;
using Cysharp.Threading.Tasks;
using Gameplay.Scripts.Utils;
using UnityEngine;

namespace Gameplay.OrderManaging
{
    public abstract class OrderRunner : IOrderRunner, IDisposable
    {
        protected CarAgent _carAgent;
        private CancellationTokenSource _cancellationTokenSource;

        private bool _isCanceled = false;

        public virtual void Init(CarAgent carAgent)
        {
            _isCanceled = false;
            _carAgent = carAgent;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public virtual async void ActivateOrder(IOrder order)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
            
           
            order.PreInteraction(_carAgent);
            _isCanceled  = await UniTask.WaitUntil(() => order.OrderBlockType == OrderBlockType.StartInteraction).AttachExternalCancellation(_cancellationTokenSource.Token).SuppressCancellationThrow();
            if (_isCanceled)
            {
                return;
            }
            order.StartInteraction();
            _isCanceled = await UniTask.WaitUntil(() => order.OrderBlockType == OrderBlockType.Process).AttachExternalCancellation(_cancellationTokenSource.Token).SuppressCancellationThrow();
            if (_isCanceled)
            {
                return;
            }
            order.Process();
            _isCanceled = await UniTask.WaitUntil(() => order.OrderBlockType == OrderBlockType.EndInteraction).AttachExternalCancellation(_cancellationTokenSource.Token).SuppressCancellationThrow();
            if (_isCanceled)
            {
                return;
            }
            order.OnEndInteraction();
            _isCanceled = await UniTask.WaitUntil(() => order.OrderBlockType == OrderBlockType.NonActivated).AttachExternalCancellation(_cancellationTokenSource.Token).SuppressCancellationThrow();
            if (_isCanceled)
            {
                return;
            }
            //CompleteOrder();
        }

        public abstract void CompleteOrder();

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
        }
    }
}