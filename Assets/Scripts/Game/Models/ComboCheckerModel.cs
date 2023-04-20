using System;
using System.Collections.Generic;
using Ability.Data;
using Ability.Views;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Models
{
    public class ComboCheckerModel
    {
        private List<FoodView> _foodTypesCombo = new List<FoodView>();
        private List<FoodView> _foodTypesLine = new List<FoodView>();
        private FoodType? _lastFood;
        public FoodType? lastFood => _lastFood;
        private bool _isStarted = false;
        public bool isStarted => _isStarted;

        private int _successfulCombos = 0;
        private float _comboTimer;
        private float _comboTimerValue = 2f;

        public Action<FoodType[]> OnComboFinish;
        public Action<int> OnFinishComboLine;
        public Action<int> OnComboProgress;
        public Action<float> OnComboTimerTic;

        public void AddFood(FoodView foodView)
        {
            _foodTypesLine.Add(foodView);
        }

        public void StartCombo(FoodType foodType)
        {
            _lastFood = foodType;
            _isStarted = true;
        }

        public int GetComboCount()
        {
            return _foodTypesCombo.Count;
        }

        public bool CheckFoodType(FoodType foodType)
        {
            if (_lastFood is FoodType.Bomb or FoodType.Magnet or FoodType.StarStick)
            {
                _lastFood = foodType;
            }
            if (foodType is FoodType.Magnet)
            {
                _lastFood = foodType;
            }
            return lastFood == foodType;
        }

        public void FinishLine()
        {
            _isStarted = false;
            _lastFood = null;
            if (_foodTypesLine.Count < 2)
            {
                for (int i = 0; i < _foodTypesLine.Count; i++)
                {
                    _foodTypesLine[i].Unselect();
                }
            }
            else
            {
                OnFinishComboLine?.Invoke(_foodTypesLine.Count);
                if (_foodTypesLine.Find(x => x.foodType == FoodType.StarStick))
                {
                    for (int i = 0; i < _foodTypesLine.Count; i++)
                    {
                        _foodTypesLine[i].MakeFresh();
                    }
                }
                if (_foodTypesLine.Find( x => x.foodType == FoodType.Bomb || x.foodType == FoodType.Ruined))
                {
                    for (int i = 0; i < _foodTypesLine.Count; i++)
                    {
                        _foodTypesLine[i].BombItem(_foodTypesCombo.Count-1);
                    }
                }
                else
                {
                    for (int i = 0; i < _foodTypesLine.Count; i++)
                    {
                        _foodTypesCombo.Add(_foodTypesLine[i]);
                        _foodTypesLine[i].TakeItem(_foodTypesCombo.Count-1);
                    }
                    _comboTimer = Time.time + _foodTypesLine.Count/_comboTimerValue + 1.5f;
                    OnComboTimerTic?.Invoke(_foodTypesLine.Count/_comboTimerValue + 1.5f);
                }
            }
            
            OnComboProgress?.Invoke(_foodTypesCombo.Count);
            _foodTypesLine.Clear();
        }
        
        public void FinishCombo()
        {
            _isStarted = false;
            _lastFood = null;
            FoodType[] foodTypes = new FoodType[_foodTypesCombo.Count];
            if (_foodTypesCombo.Count > 1)
            {
                for (int i = 0; i < _foodTypesCombo.Count; i++)
                {
                    foodTypes[i] = _foodTypesCombo[i].foodType;
                    _foodTypesCombo[i].FeedItem(i);
                }
            }

            _comboTimer = 0;
            _foodTypesCombo.Clear();
            OnComboFinish?.Invoke(foodTypes);
            OnComboProgress?.Invoke(_foodTypesCombo.Count);
        }

        public void ClearAllCombo()
        {
            _foodTypesLine.Clear();
            _foodTypesCombo.Clear();
        }

        public void ComboTimerCheck()
        {
            if (_foodTypesCombo.Count > 0 && _comboTimer > 0 && _comboTimer<Time.time)
            {
                FinishCombo();
            }
        }
    }
}