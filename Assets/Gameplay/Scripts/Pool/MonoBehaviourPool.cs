using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    [Serializable]
    public class MonoBehaviourPool<T> : BasePool<T> where T : MonoBehaviour
    {
        protected List<T> _poolList = new List<T>();
        
        public override T GetObject()
        {
            foreach (var poolObject in _poolList)
            {
                if (poolObject.gameObject == null)
                {
                    DeleteObject(poolObject);
                    return GetObject();
                }
                if (poolObject.gameObject.activeSelf == false)
                {
                    poolObject.gameObject.SetActive(true);
                    return poolObject;
                }
            }
            var newPoolObject = MonoBehaviour.Instantiate(prefab, parent.transform).GetComponent<T>();
            _poolList.Add(newPoolObject);
            return newPoolObject;
        }

        private void DeleteObject(T poolObject)
        {
            _poolList.Remove(poolObject);
        }

        public override void ReturnObject(T poolObject)
        { 
            poolObject.gameObject.SetActive(false);
            ((IPoolable)poolObject).Return();
        }

        public void ReturnAll()
        {
            foreach (var monoBehaviour in _poolList)
            {
                ReturnObject(monoBehaviour);
            }
        }

        public override void ReleaseAll()
        {
            foreach (var monoBehaviour in _poolList)
            {
                ((IPoolable)monoBehaviour).Release();
            }
            _poolList.Clear();
        }
    }
}