using System;
using DG.Tweening;
using Game.Models;
using Level.Views;
using Pets.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using YG;
using Sequence = DG.Tweening.Sequence;

namespace UI.Views
{
    public class UIMainMenu : UIView
    {
        [SerializeField] private Button _leftPetButton, _rightPetButton;
        [SerializeField] private Button _startGameButton;
        [SerializeField] private UIPetStatView _petHeight;
        [SerializeField] private UIPetStatView _petWeight;
        [SerializeField] private UIPetFoodView _petFavouriteFood;
        [SerializeField] private UIPetFoodView _petUnlovedFood;
        [SerializeField] private UIPetLevel _uiPetLevel;
        [SerializeField] private UILevelUpText _uiLevelUpText;
        [SerializeField] private Text _bestScore;

        private AudioSource _audioSource;

        private Camera _camera;
        private PetPlace _petPlace;
        private Sequence _sequence;

        private void Awake()
        {
            _camera = Camera.main;
            _audioSource = GetComponent<AudioSource>();
        }

        public void Initialize(GameInfo gameInfo, PetPlace petPlace)
        {
            _petPlace = petPlace;
            _startGameButton.onClick.AddListener(gameInfo.StartGame);
            
            gameInfo.OnStartGame += Hide;
            gameInfo.OnLoseGame += Show;
            gameInfo.OnWinGame += UpdateBestScore;
            gameInfo.OnChangePetModel += UpdatePetStats;
            gameInfo.OnPetLevelUp += PetLevelUp;
            UpdateBestScore();
            _leftPetButton.onClick.AddListener(gameInfo.PrevPetModel);
            _rightPetButton.onClick.AddListener(gameInfo.NextPetModel);
        }

        private void FixedUpdate()
        {
            if (_petPlace == null)return;
            
            _leftPetButton.transform.position = _camera.WorldToScreenPoint(_petPlace.GetFlyPosition()+Vector3.down+Vector3.left*1.5f);
            _rightPetButton.transform.position = _camera.WorldToScreenPoint(_petPlace.GetFlyPosition()+Vector3.down+Vector3.right*1.5f);
            _petHeight.transform.position = _camera.WorldToScreenPoint(_petPlace.GetFlyPosition()+Vector3.up*2f+Vector3.left);
            _petWeight.transform.position = _camera.WorldToScreenPoint(_petPlace.GetFlyPosition()+Vector3.up*2f+Vector3.right);
            _petUnlovedFood.transform.position = _camera.WorldToScreenPoint(_petPlace.GetFlyPosition()+Vector3.up*3f);
            _petFavouriteFood.transform.position = _camera.WorldToScreenPoint(_petPlace.GetFlyPosition()+Vector3.up*4f);
            _uiPetLevel.transform.position = _camera.WorldToScreenPoint(_petPlace.GetFlyPosition()-Vector3.up*2f);
        }

        private void UpdateBestScore()
        {
            _bestScore.text = YandexGame.savesData.bestScore.ToString();
        }

        private void UpdatePetStats(PetModel petModel)
        {
            _sequence.Kill();
            _sequence = DOTween.Sequence();
            _sequence
                .Append(_startGameButton.transform.DOScale(1.2f, 1f))
                .Append(_startGameButton.transform.DOScale(1f, 1f))
                .SetLoops(-1)
                .SetEase(Ease.OutBack);

            _petHeight.UpdateStatValue(petModel.GetScale());
            _petWeight.UpdateStatValue(petModel.GetWeight());
            _petFavouriteFood.UpdateStatValue(petModel.favoriteFood);
            _petUnlovedFood.UpdateStatValue(petModel.unlovedFood);
            _uiPetLevel.UpdatePetStar(petModel.level);
        }

        private void PetLevelUp(PetModel petModel)
        {
            Show();
            _petHeight.UpdateStatValue(petModel.GetScale());
            _petWeight.UpdateStatValue(petModel.GetWeight());
            _petFavouriteFood.UpdateStatValue(petModel.favoriteFood);
            _petUnlovedFood.UpdateStatValue(petModel.unlovedFood);
            _uiPetLevel.UpPetStar(petModel.level);
            _uiLevelUpText.ShowLevelUp();
            _audioSource.Play();
        }
    }
}