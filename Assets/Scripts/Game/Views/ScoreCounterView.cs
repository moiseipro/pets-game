using System;
using Game.Models;
using TMPro;
using UnityEngine;

namespace Game.Views
{
    public class ScoreCounterView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;
        private float _oldScore = 0, _newScore;

        public void Initialize(GameInfo _gameInfo)
        {
            _gameInfo.OnScoreUpdate += ScoreUpdate;
        }

        private void ScoreUpdate(int value)
        {
            _newScore = value;
        }
        
        private void Update()
        {
            _oldScore = Mathf.Lerp(_oldScore, _newScore, Time.deltaTime * 5f);
            _scoreText.text = Mathf.Round(_oldScore).ToString();
        }
    }
}