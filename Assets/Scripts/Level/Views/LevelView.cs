using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ability.Data;
using Ability.Views;
using Effects.Views;
using Game.Models;
using UnityEngine;
using YG;
using Random = UnityEngine.Random;

namespace Level.Views
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private LineView _lineView;
        [SerializeField] private FoodView[] _foodViewsPrefab;
        [SerializeField] private ParticleEffectPool _deathParticleEffectPool;
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private FoodPool _foodPool;
        [SerializeField] private Vector2 _petPlaceOffset;
        private PetFlask _currentPetFlask;
        private ComboCheckerModel _comboCheckerModel;
        private GameInfo _gameInfo;
        private int _preBonus = 0;
        private int _bonusFood = 0;

        private Transform _transform;

        private Coroutine _coroutineTimer;
        private Coroutine _coroutineSpawner;
        private FoodType[] _foodTypesForSpawnFavourite;
        private FoodType[] _foodTypesForSpawnUnloved;

        private void Awake()
        {
            _transform = transform;
        }

        public void Initialize(GameInfo gameInfo, ComboCheckerModel comboCheckerModel, PetFlask petFlask)
        {
            _gameInfo = gameInfo;
            _comboCheckerModel = comboCheckerModel;
            _comboCheckerModel.OnFinishComboLine += ComboSpawnerFood;
            _currentPetFlask = petFlask;
            _currentPetFlask.SetLocalPosition(_petPlaceOffset);
            _gameInfo.OnChangePetModel += _currentPetFlask.SetPetModel;
            YandexGame.RewardVideoEvent += RewardSpawn;
        }
        
        public void PlayLevel()
        {
            _coroutineTimer = StartCoroutine(SpawnFoodTimer());
            _foodTypesForSpawnFavourite = _gameInfo.GetPetFavouriteFood();
            _foodTypesForSpawnUnloved = _gameInfo.GetPetUnlovedFood();
            _currentPetFlask.ResetWaterPosition();
            _foodPool.Initialize(_lineView, _comboCheckerModel, _deathParticleEffectPool, _currentPetFlask, _gameInfo);
        }

        private void ComboSpawnerFood(int count)
        {
            _coroutineSpawner = StartCoroutine(SpawnFoodByCount(count));
        }

        private IEnumerator SpawnFoodTimer()
        {
            while (true)
            {
                yield return new WaitForSeconds(_gameInfo.timeToSpawnFood/Mathf.Clamp(_bonusFood, 1, _bonusFood));

                SpawnFood();
            }
        }

        private IEnumerator SpawnFoodByCount(int count)
        {
            _bonusFood += count / 2;
            for (int i = 0; i < count/2; i++)
            {
                yield return new WaitForSeconds(0.1f);
                SpawnFood();
            }
        }

        private void RewardSpawn(int id)
        {
            if (id == 0)
            {
                _bonusFood += 15;
            }
        }

        private FoodType GetRandomFavouriteFood()
        {
            FoodType randomFoodType = FoodType.Ruined;
            while (!_foodTypesForSpawnFavourite.Contains(randomFoodType))
            {
                randomFoodType = (FoodType)Random.Range(0,Enum.GetValues(typeof(FoodType)).Length - 1 - (_preBonus % 5 == 0 ? Random.Range(0,4) : 3));
            }
            return randomFoodType;
        }

        private FoodType GetRandomUnlovedFood()
        {
            FoodType randomFoodType = FoodType.Ruined;
            while (!_foodTypesForSpawnUnloved.Contains(randomFoodType))
            {
                randomFoodType = (FoodType)Random.Range(0,Enum.GetValues(typeof(FoodType)).Length - 1 - (_preBonus % 5 == 0 ? Random.Range(0,4) : 3));
            }

            return randomFoodType;
        }
        
        private FoodType GetRandomFood()
        {
            _preBonus++;
            FoodType randomFoodType = GetRandomFavouriteFood();
            if (_preBonus % 2 == 0 || _bonusFood > 0)
            {
                if (_bonusFood > 0) _bonusFood--;
                randomFoodType = GetRandomFavouriteFood();
            }
            else
            {
                randomFoodType = GetRandomUnlovedFood();
            }
            
            return randomFoodType;
        }
        
        private void SpawnFood()
        {
            FoodView foodView = _foodPool.Pool.Get();
            
            foodView.Respawn(_spawnPoints[Random.Range(0, _spawnPoints.Length)].position, _gameInfo.spoilingTimeMod, GetRandomFood());
        }

        public void StopLevel()
        {
            _lineView.ResetPoints();
            _comboCheckerModel.ClearAllCombo();
            StopCoroutine(_coroutineTimer);
            if (_coroutineSpawner != null)
            {
                StopCoroutine(_coroutineSpawner);
            }
        }
    }
}