using System;
using DG.Tweening;
using Game.Models;
using Level.Views;
using Pets.Data;
using Pets.Models;
using UI.Views;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

namespace Game.Views
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private PetFlask _petFlaskPrefab;
        [SerializeField] private PetPlace _petPlacePrefab;
        [SerializeField] private PetModelScriptable[] _petModelScriptable;
        private PetFlask _currentPetFlask;
        private PetPlace _currentPetPlace;
        private GameInfo _gameInfo;
        private ComboCheckerModel _comboCheckerModel;
        
        private LevelView _levelView;
        private MenuView _menuView;
        private ScoreCounterView _scoreCounterView;

        private UIInitializer _uiInitializer;
        private CameraView _cameraView;
        private GameOverLineView _gameOverLineView;


        private void Start()
        {
            _cameraView = Camera.main.GetComponent<CameraView>();
            
            
            if (YandexGame.SDKEnabled)
            {
                LoadGameInfo();
            }
            else
            {
                YandexGame.GetDataEvent += LoadGameInfo;
            }
            
            
        }

        private void StartGame()
        {
            YandexGame.FullscreenShow();
            _cameraView.SetTarget(_levelView.transform);
            _levelView.PlayLevel();
        }

        private void StopGame()
        {
            _cameraView.SetTarget(_menuView.transform);
            _levelView.StopLevel();
            SaveGameInfo();
        }

        private void FixedUpdate()
        {
            if (_comboCheckerModel != null)
            {
                _comboCheckerModel.ComboTimerCheck();
            }
        }

        private void LoadGameInfo()
        {
            string json = YandexGame.savesData.gameInfo;
            GameInfo gameInfo = JsonUtility.FromJson<GameInfo>(json);
            
            if (gameInfo == null)
            {
                gameInfo = new GameInfo();
            }
            if (gameInfo.GetPetCount() == 0)
            {
                foreach (var petModelScriptable in _petModelScriptable)
                {
                    gameInfo.AddPetModel(new PetModel(petModelScriptable.PetModel));
                }
            }

            _gameInfo = gameInfo;
            _gameInfo.OnStartGame += StartGame;
            _gameInfo.OnLoseGame += StopGame;
            _gameInfo.OnWinGame += StopGame;
            
            _comboCheckerModel = new ComboCheckerModel();
            _comboCheckerModel.OnComboFinish += _gameInfo.UpdateScoreByFood;

            _levelView = FindObjectOfType<LevelView>();
            _currentPetFlask = Instantiate(_petFlaskPrefab, Vector3.zero, Quaternion.identity, _levelView.transform);
            _levelView.Initialize(_gameInfo, _comboCheckerModel, _currentPetFlask);
            _menuView = FindObjectOfType<MenuView>();
            _currentPetPlace = Instantiate(_petPlacePrefab, Vector3.zero, Quaternion.identity, _menuView.transform);
            _menuView.Initialize(_gameInfo, _currentPetPlace);
            _uiInitializer = FindObjectOfType<UIInitializer>();
            _uiInitializer.Initialize(_comboCheckerModel, _gameInfo, _currentPetPlace);
            _cameraView.SetTarget(_menuView.transform);
            _gameOverLineView = FindObjectOfType<GameOverLineView>();
            _gameOverLineView.Initialize(_gameInfo);
            _scoreCounterView = FindObjectOfType<ScoreCounterView>();
            _scoreCounterView.Initialize(_gameInfo);
            
            
            _gameInfo.SelectPetModel();
        }
        
        public void SaveGameInfo()
        {
            YandexGame.savesData.gameInfo = JsonUtility.ToJson(_gameInfo);
            YandexGame.SaveProgress();
        }

        public void DeleteSave()
        {
            DOTween.KillAll();
            PlayerPrefs.DeleteAll();
            _gameInfo = null;
            SaveGameInfo();
            SceneManager.LoadScene(0);
        }
    }
}