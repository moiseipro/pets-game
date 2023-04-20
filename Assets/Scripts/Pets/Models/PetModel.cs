using System;
using System.Linq;
using Ability.Data;
using UnityEngine;

namespace Pets.Models
{
    [Serializable]
    public class PetModel
    {
        [SerializeField] private string _name;
        [SerializeField] private float _height;
        public float height => _height;
        [SerializeField] private float _weight;
        public float weight => _weight;
        [SerializeField] private FoodType[] _favoriteFood;
        public FoodType[] favoriteFood => _favoriteFood;
        [SerializeField] private FoodType[] _unlovedFood;
        public FoodType[] unlovedFood => _unlovedFood;
        [SerializeField] private float _dirty;
        public float dirty => _dirty;
        [SerializeField] private float _hungry;
        public float hungry => _hungry;
        [SerializeField] private int _petSprite;
        public int petSprite => _petSprite;
        // [SerializeField] private Sprite _sprite;
        // public Sprite sprite => _sprite;
        [SerializeField] private int _level = 1;
        public int level => _level;
        [SerializeField] private int _maxLevel;
        public int maxLevel => _maxLevel;

        public Action<float> OnFeed;
        public Action OnLevelUp;

        public PetModel(PetModel petModel)
        {
            _name = petModel._name;
            _height = petModel._height;
            _weight = petModel._weight;
            _favoriteFood = petModel._favoriteFood;
            _unlovedFood = petModel._unlovedFood;
            _dirty = petModel._dirty;
            //_sprite = petModel._sprite;
            _petSprite = petModel._petSprite;
            _level = petModel._level;
            _maxLevel = petModel._maxLevel;
            _hungry = GetWeight();
        }

        public float GetScale()
        {
            return MathF.Round(_height/Mathf.Clamp(_maxLevel-_level, 1, _maxLevel), 1)+1f;
        }

        public float GetWeight()
        {
            return MathF.Round(_weight/Mathf.Clamp(_maxLevel-_level, 1, _maxLevel), 1);
        }

        public void StartFeeding()
        {
            _hungry = GetWeight();
        }

        public bool LevelUp()
        {
            if (_level < _maxLevel)
            {
                _level++;
                _hungry = GetWeight();
                OnLevelUp?.Invoke();
                return true;
            }
            return false;
        }

        public bool Feed(FoodType[] foodTypes)
        {
            foreach (var foodType in foodTypes)
            {
                _dirty += 0.01f;
                if (_unlovedFood.Contains(foodType))
                {
                    _hungry += 0.005f*foodTypes.Length;
                } 
                else if (_favoriteFood.Contains(foodType))
                {
                    _hungry -= 0.005f*foodTypes.Length;
                }
                else
                {
                    if (foodType == FoodType.Ruined)
                    {
                        _hungry += 0.01f*foodTypes.Length;
                    }
                    else
                    {
                        _hungry -= 0.001f*foodTypes.Length;
                    }
                    
                }
            }

            
            _hungry = Mathf.Clamp(_hungry, 0, GetWeight());
            var isFull = _hungry == 0;
            OnFeed?.Invoke(1f-_hungry/GetWeight());
            
            return isFull;

        }

        public float GetSatiety()
        {
            return 1f-_hungry/GetWeight();
        }
    }
}