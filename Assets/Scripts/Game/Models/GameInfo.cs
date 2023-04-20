using System;
using System.Collections.Generic;
using Ability.Data;
using Pets.Models;
using UnityEngine;
using YG;

namespace Game.Models
{
    [Serializable]
    public class GameInfo
    {
        [SerializeField] private List<PetModel> _collectedPetModels;
        [SerializeField] private int _selectedPet = 0;
        private float _spoilingTimeMod = 11f;
        public float spoilingTimeMod => _spoilingTimeMod-GetPetModel().level/2f;
        private float _timeToSpawnFood = 1.6f;
        public float timeToSpawnFood => Mathf.Clamp(_timeToSpawnFood-GetPetModel().level/GetPetModel().maxLevel-GetPetModel().GetSatiety(),0.9f,_timeToSpawnFood);
        private int _moneyStar;
        private DateTime _lastCheckDateTime;

        private int _currentScore = 0;

        public Action OnLoseGame;
        public Action OnStartGame;
        public Action OnWinGame;
        public Action<PetModel> OnChangePetModel;
        public Action<PetModel> OnPetLevelUp;
        public Action<int> OnScoreUpdate;

        public GameInfo()
        {
            _collectedPetModels = new List<PetModel>();
        }
        
        public void AddPetModel(PetModel petModel)
        {
            _collectedPetModels.Add(petModel);
        }

        public void ClearPetCollection()
        {
            _collectedPetModels.Clear();
        }

        public int GetPetCount()
        {
            return _collectedPetModels.Count;
        }

        public void SelectPetModel()
        {
            OnChangePetModel?.Invoke(_collectedPetModels[_selectedPet]);
        }

        public void NextPetModel()
        {
            _selectedPet++;
            if (_selectedPet > _collectedPetModels.Count-1)
            {
                _selectedPet = 0;
            }

            SelectPetModel();
        }

        public void PrevPetModel()
        {
            _selectedPet--;
            if (_selectedPet < 0)
            {
                _selectedPet = _collectedPetModels.Count-1;
            }
            SelectPetModel();
        }

        private PetModel GetPetModel()
        {
            return _collectedPetModels[_selectedPet];
        }

        public FoodType[] GetPetFood()
        {
            PetModel petModel = GetPetModel();
            List<FoodType> foodTypes = new List<FoodType>();
            foodTypes.AddRange(petModel.favoriteFood);
            foodTypes.AddRange(petModel.unlovedFood);
            foodTypes.AddRange(new FoodType[]{FoodType.Bomb, FoodType.Magnet, FoodType.StarStick});
            return foodTypes.ToArray();
        }
        
        public FoodType[] GetPetFavouriteFood()
        {
            PetModel petModel = GetPetModel();
            List<FoodType> foodTypes = new List<FoodType>();
            foodTypes.AddRange(petModel.favoriteFood);
            foodTypes.AddRange(new FoodType[]{FoodType.Bomb, FoodType.Magnet, FoodType.StarStick});
            return foodTypes.ToArray();
        }
        public FoodType[] GetPetUnlovedFood()
        {
            PetModel petModel = GetPetModel();
            List<FoodType> foodTypes = new List<FoodType>();
            foodTypes.AddRange(petModel.unlovedFood);
            foodTypes.AddRange(new FoodType[]{FoodType.Bomb, FoodType.Magnet, FoodType.StarStick});
            return foodTypes.ToArray();
        }

        public void UpdateScoreByFood(FoodType[] foodTypes)
        {
            PetModel currentPetModel = GetPetModel();
            _currentScore+=foodTypes.Length;
            OnScoreUpdate?.Invoke(_currentScore);
            if (currentPetModel.Feed(foodTypes))
            {
                GetPetModel().LevelUp();
                OnPetLevelUp?.Invoke(GetPetModel());
                if (YandexGame.savesData.bestScore < _currentScore)
                {
                    YandexGame.savesData.bestScore = _currentScore;
                    YandexGame.NewLeaderboardScores("food", _currentScore);
                }
                OnWinGame?.Invoke();
            }
        }

        public void StartGame()
        {
            PetModel currentPetModel = GetPetModel();
            currentPetModel.StartFeeding();
            _currentScore=0;
            OnScoreUpdate?.Invoke(_currentScore);
            OnStartGame?.Invoke();
        }

        public void LoseGame()
        {
            Debug.Log("Lose Game!");
            OnLoseGame?.Invoke();
        }
    }
}