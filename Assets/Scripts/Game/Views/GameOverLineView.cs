using System;
using Ability.Views;
using Game.Models;
using TMPro;
using UnityEngine;

namespace Game.Views
{
    public class GameOverLineView : MonoBehaviour
    {
        [SerializeField] private float _gameOverTime = 3f;
        private int _foodCount;
        private GameInfo _gameInfo;
        private float _gameOverTimer;
        private TMP_Text _text;

        private void Awake()
        {
            _text = GetComponentInChildren<TMP_Text>();
        }

        public void Initialize(GameInfo gameInfo)
        {
            _gameInfo = gameInfo;
        }
        private void OnTriggerStay2D(Collider2D col)
        {
            if (_foodCount != 0)
            {
                Debug.Log("stayed");
                if (_gameOverTimer == 0)
                {
                    _gameOverTimer = Time.time + _gameOverTime;
                }

                if (_gameOverTimer - Time.time < _gameOverTime-0.5f)
                {
                    _text.color = Color.red;
                }
                else
                {
                    _text.color = Color.white;
                }
                if (_gameOverTimer < Time.time)
                {
                    _gameInfo.LoseGame();
                    _gameOverTimer = 0;
                    _text.color = Color.white;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.GetComponentInParent<FoodView>())
            {
                _foodCount++;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.GetComponentInParent<FoodView>())
            {
                _foodCount--;
                if (_foodCount == 0)
                {
                    _gameOverTimer = 0;
                }
            }
        }
    }
}