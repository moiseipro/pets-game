using System;
using Game.Models;
using UnityEngine;

namespace Level.Views
{
    public class MenuView : MonoBehaviour
    {
        [SerializeField] private Vector2 _petPlaceOffset;

        private GameInfo _gameInfo;
        private PetPlace _currentPetPlace;
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }
        
        public void Initialize(GameInfo gameInfo, PetPlace petPlace)
        {
            _gameInfo = gameInfo;
            _currentPetPlace = petPlace;
            _currentPetPlace.SetLocalPosition(_petPlaceOffset);
            _gameInfo.OnChangePetModel += _currentPetPlace.SetPetModel;
        }
    }
}