using System;
using DG.Tweening;
using Pets.Models;
using UnityEngine;

namespace Pets.Views
{
    public class PetView : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private Transform _transform;
        private Sequence _sequence;
        [SerializeField] private Sprite[] _petSprite;
        
        private PetModel _currentPetModel;

        private void Awake()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _transform = transform;
        }

        public void SetPetModel(PetModel petModel)
        {
            _sequence.Kill();
            _sequence = DOTween.Sequence();
            _sequence
                .Append(_transform.DOScale(0, 0.2f))
                .AppendCallback(ChangeSkin)
                .SetEase(Ease.OutBack);
            if (_currentPetModel != null)
            {
                _currentPetModel.OnFeed -= OnFeedAnimation;
            }
            _currentPetModel = petModel;
            _currentPetModel.OnFeed += OnFeedAnimation;
            _currentPetModel.OnLevelUp += LevelUp;
            

            _sequence
                .Append(_transform.DOScale(_currentPetModel.GetScale(), 1f))
                .SetEase(Ease.InOutBack);
        }

        private void ChangeSkin()
        {
            _spriteRenderer.sprite = _petSprite[Mathf.Clamp(_currentPetModel.petSprite, 0 , _petSprite.Length)];
        }
        
        private void LevelUp()
        {
            _sequence.Kill();
            _sequence = DOTween.Sequence();
            float newScale = _currentPetModel.GetScale();
            _sequence
                .Append(_transform.DOScale(newScale * 1.5f, 1f))
                .Append(_transform.DOScale(newScale, 1f))
                .SetEase(Ease.InOutBack);
        }
        
        private void OnFeedAnimation(float value)
        {
            _sequence.Kill();
            _sequence = DOTween.Sequence();
            _sequence
                .Append(_transform.DOShakeRotation(value+0.5f, new Vector3(0,0, 10f), 10))
                .SetEase(Ease.InOutBack);
        }
    }
}