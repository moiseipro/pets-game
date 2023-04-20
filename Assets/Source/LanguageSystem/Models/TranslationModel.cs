using System;
using Source.LanguageSystem.Data;

namespace Source.LanguageSystem.Models
{
    [Serializable]
    public class TranslationModel
    {
        public LanguageTypes languageType;
        public string translatedText;
    }
}