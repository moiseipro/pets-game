using System;
using Game.Models;
using Level.Views;
using Pets.Models;
using Source.TutorialSystem.Views;
using UnityEngine;

namespace UI.Views
{
    public class UIInitializer : MonoBehaviour
    {
        [SerializeField] private TutorialTarget _tutorialTarget;
        
        private UIComboView _uiComboView;
        private UIComboTimerView _uiComboTimerView;
        private UIMainMenu _uiMainMenu;
        private UISatietyView _uiSatietyView;
        private UIRewardedButton _uiRewardedButton;

        private ComboCheckerModel _comboCheckerModel;
        private GameInfo _gameInfo;
        private PetModel _currentPetModel;

        private void Awake()
        {
            _uiComboView = GetComponentInChildren<UIComboView>();
            _uiComboView.Hide();
            _uiComboTimerView = GetComponentInChildren<UIComboTimerView>();
            _uiComboTimerView.Hide();
            _uiMainMenu = GetComponentInChildren<UIMainMenu>();
            _uiRewardedButton = GetComponentInChildren<UIRewardedButton>();
            _uiRewardedButton.Hide();
            //_uiSatietyView = GetComponentInChildren<UISatietyView>();
            //_uiSatietyView.Hide();
        }

        public void Initialize(ComboCheckerModel comboCheckerModel, GameInfo gameInfo, PetPlace petPlace)
        {
            _comboCheckerModel = comboCheckerModel;
            _comboCheckerModel.OnComboProgress += _uiComboView.UpdateCombo;
            _comboCheckerModel.OnComboTimerTic += _uiComboTimerView.UpdateComboTimer;
            _gameInfo = gameInfo;
            //_gameInfo.OnChangePetModel += SubscribePetModel;
            _uiMainMenu.Initialize(_gameInfo, petPlace);
            _uiRewardedButton.Initialize(_gameInfo);
            _tutorialTarget.StartTutorial();
        }

        // private void SubscribePetModel(PetModel petModel)
        // {
        //     if (_currentPetModel != null)
        //     {
        //         _currentPetModel.OnFeed -= _uiSatietyView.UpdateSatietyBar;
        //     }
        //     _currentPetModel = petModel;
        //     _currentPetModel.OnFeed += _uiSatietyView.UpdateSatietyBar;
        // }

        private void OnDisable()
        {
            if (_comboCheckerModel != null)
            {
                _comboCheckerModel.OnComboProgress -= _uiComboView.UpdateCombo;
                _comboCheckerModel.OnComboTimerTic -= _uiComboTimerView.UpdateComboTimer;
            }
        }
    }
}