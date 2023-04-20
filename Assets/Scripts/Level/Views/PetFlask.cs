using System;
using Pets.Models;
using Pets.Views;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Level.Views
{
    public class PetFlask : PetPlace
    {
        [SerializeField] private Vector2 _minPosition, _maxPosition;
        [SerializeField] private Transform _waterTransform;
        
        public override void SetPetModel(PetModel petModel)
        {
            if (_petModel != null)
            {
                _petModel.OnFeed -= UpdateWater;
            }
            base.SetPetModel(petModel);
            _petModel.OnFeed += UpdateWater;
        }

        public void ResetWaterPosition()
        {
            _waterTransform.localPosition = _minPosition;
        }

        private void UpdateWater(float value)
        {
            _waterTransform.localPosition = (_minPosition-(_minPosition-_maxPosition)*value);
        }
    }
}