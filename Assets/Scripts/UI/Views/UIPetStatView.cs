using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class UIPetStatView : MonoBehaviour
    {
        [SerializeField] private Text _valueText;
        private float _oldValue = 0f;
        private float _newValue = 0f;

        public void UpdateStatValue(float value)
        {
            _newValue = value;
        }

        private void Update()
        {
            _oldValue = Mathf.Lerp(_oldValue, _newValue, Time.deltaTime * 5f);
            _valueText.text = MathF.Round(_oldValue, 1).ToString();
        }
    }
}