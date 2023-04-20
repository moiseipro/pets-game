using Effects.Data;
using UnityEngine;
using UnityEngine.Pool;

namespace Effects.Views
{
    public class AudioEffectPool : MonoBehaviour
    {
        public PoolType poolType;

        // Collection checks will throw errors if we try to release an item that is already in the pool.
        public bool collectionChecks = true;
        public int maxPoolSize = 10;

        public AudioEffect audioEffectPrefab;

        IObjectPool<AudioEffect> m_Pool;

        public IObjectPool<AudioEffect> Pool
        {
            get
            {
                if (m_Pool == null)
                {
                    if (poolType == PoolType.Stack)
                        m_Pool = new ObjectPool<AudioEffect>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
                    else
                        m_Pool = new LinkedPool<AudioEffect>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, maxPoolSize);
                }
                return m_Pool;
            }
        }

        AudioEffect CreatePooledItem()
        {
            AudioEffect audioEffect = Instantiate(audioEffectPrefab);
            
            audioEffect.pool = Pool;

            return audioEffect;
        }

        // Called when an item is returned to the pool using Release
        void OnReturnedToPool(AudioEffect system)
        {
            system.gameObject.SetActive(false);
        }

        // Called when an item is taken from the pool using Get
        void OnTakeFromPool(AudioEffect system)
        {
            system.gameObject.SetActive(true);
        }

        // If the pool capacity is reached then any items returned will be destroyed.
        // We can control what the destroy behavior does, here we destroy the GameObject.
        void OnDestroyPoolObject(AudioEffect system)
        {
            Destroy(system.gameObject);
        }
    }
}