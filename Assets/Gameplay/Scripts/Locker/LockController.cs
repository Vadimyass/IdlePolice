using System;
using System.Collections.Generic;

namespace Gameplay.Scripts.Locker
{
    public class LockController
    {
        private readonly Dictionary<Type, HashSet<LockerBase>> _lockers = new ();

        public bool HaveLock<T>() where T : LockerBase => _lockers.ContainsKey(typeof(T)) && _lockers[typeof(T)].Count > 0;

        public T AddLock<T>() where T : LockerBase, new()
        {
            var locker = new T();

            if(!_lockers.ContainsKey(typeof(T)))
            {
                var lockers = new HashSet<LockerBase> {locker};
                locker.Configurate(this);
                
                _lockers.Add(locker.GetType(), lockers);
            }
            else
            {
                var lockers = _lockers[typeof(T)];
                lockers.Add(locker);
                locker.Configurate(this);
            }

            return locker;
        }

        public void RemoveLock(LockerBase locker)
        {
            var lockers = _lockers[locker.GetType()];
            lockers.Remove(locker);
        }

        public void ResetLockers()
        {
            _lockers.Clear();
        }
    }
}