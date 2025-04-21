using System;

namespace Gameplay.Scripts.Locker
{
    public abstract class LockerBase : IDisposable
    {
        protected LockController LockController { get; private set; }

        public virtual void Configurate(LockController lockController)
        {
            LockController = lockController;
        }

        public void Dispose()
        {
            LockController.RemoveLock(this);
        }
    }
}