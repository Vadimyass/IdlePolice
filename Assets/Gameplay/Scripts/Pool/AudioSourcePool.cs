using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    [Serializable]
    public class AudioSourcePool : BasePool<AudioSource>
    {
        private List<AudioSource> _poolList = new List<AudioSource>();
        
        public override AudioSource GetObject()
        {
            foreach (var poolObject in _poolList)
            {
                if (poolObject!= null && poolObject.gameObject.activeInHierarchy == false)
                {
                    poolObject.gameObject.SetActive(true);
                    return poolObject;
                }
            }
            return CreatePoolObject();
        }

        public AudioSource FindObjectByAudioClip(AudioClip audioClip)
        {
            foreach (var audioSource in _poolList)
            {
                if (audioSource.clip == audioClip && audioSource.gameObject.activeSelf == true)
                {
                    return audioSource;
                }
            }
            return null;
        }

        public IReadOnlyList<AudioSource> GetPoolList()
        {
            return _poolList;
        }

        public void CreateAudioSources(int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                CreatePoolObject();
            }
        }

        private AudioSource CreatePoolObject()
        {
            var newPoolObject = MonoBehaviour.Instantiate(prefab, parent.transform).GetComponent<AudioSource>();
            _poolList.Add(newPoolObject);
            ReturnObject(newPoolObject);
            return newPoolObject;
        }
        
        public override void ReturnObject(AudioSource poolObject)
        {
            if(poolObject == null) return;
            
            poolObject.gameObject.SetActive(false);
        }

        public override void ReleaseAll()
        {
            _poolList.Clear();
        }
    }
}