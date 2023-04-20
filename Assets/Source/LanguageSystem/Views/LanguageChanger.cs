using System;
using System.Linq;
using GamePush;
using Source.LanguageSystem.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Source.LanguageSystem.Views
{
    public class LanguageChanger : MonoBehaviour
    {
        public TranslationModel[] translationModels;
        private TranslationModel _currentTranslation;
        private Text _text;

        private void Start()
        {
            OnSDKReady();
        }

        private void OnEnable()
        {
            _text = GetComponent<Text>();
            GP_SDK.OnReady += OnSDKReady;
        }

        private void OnDisable()
        {
            GP_SDK.OnReady -= OnSDKReady;
        }

        private void OnSDKReady()
        {
            _currentTranslation = translationModels.First(x => x.languageType.ToString() == GP_Language.Current());
            _text.text = _currentTranslation.translatedText;
        }
    }
}