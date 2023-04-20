using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class UISettingsButton : UIView
    {
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Toggle _soundOnOff;
        [SerializeField] private AudioSource _gameMusic;

        private void Awake()
        {
            Hide();
            _musicSlider.onValueChanged.AddListener(GameMusicChange);
            _soundOnOff.onValueChanged.AddListener(SoundOnOff);
            _musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", _gameMusic.volume);
            _soundOnOff.isOn = PlayerPrefs.GetInt("Sound", 1) == 1;
        }

        private void GameMusicChange(float value)
        {
            _gameMusic.volume = value;
            PlayerPrefs.SetFloat("MusicVolume", value);
        }
        
        private void SoundOnOff(bool value)
        {
            int sound = value ? 1 : 0;
            AudioListener.volume = sound;
            PlayerPrefs.SetInt("Sound", sound);
        }
    }
}