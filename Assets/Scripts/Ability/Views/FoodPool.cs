using Effects.Data;
using Effects.Views;
using Game.Models;
using Level.Views;
using UnityEngine;
using UnityEngine.Pool;

namespace Ability.Views
{
    public class FoodPool : MonoBehaviour
    {
        public PoolType poolType;

        // Collection checks will throw errors if we try to release an item that is already in the pool.
        public bool collectionChecks = true;
        public int maxPoolSize = 10;

        public FoodView foodViewPrefab;

        IObjectPool<FoodView> m_Pool;
        
        private LineView _lineView;
        private ComboCheckerModel _comboCheckerModel;
        private ParticleEffectPool _deathEffectPool;
        private PetFlask _petFlask;
        private GameInfo _gameInfo;

        public IObjectPool<FoodView> Pool
        {
            get
            {
                if (m_Pool == null)
                {
                    if (poolType == PoolType.Stack)
                        m_Pool = new ObjectPool<FoodView>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
                    else
                        m_Pool = new LinkedPool<FoodView>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, maxPoolSize);
                }
                return m_Pool;
            }
        }

        public void Initialize(LineView lineView, ComboCheckerModel comboCheckerModel, ParticleEffectPool deathEffectPool, PetFlask petFlask, GameInfo gameInfo)
        {
            _lineView = lineView;
            _comboCheckerModel = comboCheckerModel;
            _deathEffectPool = deathEffectPool;
            _petFlask = petFlask;
            _gameInfo = gameInfo;
        }
        
        FoodView CreatePooledItem()
        {
            FoodView foodView = Instantiate(
                foodViewPrefab, 
                Vector3.zero, 
                Quaternion.identity,
                transform);
            foodView.Initialize(_lineView, _comboCheckerModel, _deathEffectPool, _petFlask, _gameInfo);
            
            foodView.pool = Pool;

            return foodView;
        }

        // Called when an item is returned to the pool using Release
        void OnReturnedToPool(FoodView system)
        {
            system.gameObject.SetActive(false);
        }

        // Called when an item is taken from the pool using Get
        void OnTakeFromPool(FoodView system)
        {
            system.gameObject.SetActive(true);
        }

        // If the pool capacity is reached then any items returned will be destroyed.
        // We can control what the destroy behavior does, here we destroy the GameObject.
        void OnDestroyPoolObject(FoodView system)
        {
            Destroy(system.gameObject);
        }
    }
}