using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class UIComboView : UIView
    {
        private Text _comboText;
        private Sequence _sequence;

        private void Awake()
        {
            _comboText = GetComponentInChildren<Text>();
        }

        public void UpdateCombo(int value)
        {
            _sequence = DOTween.Sequence();
            if (value > 1)
            {
                Show();
                _sequence
                    .Append(transform.DOScale(Vector3.one * (0.5f + value / 20f), 0.5f))
                    .SetEase(Ease.InOutBack);
                _comboText.text = "x" + value;
            }
            else
            {
                _sequence
                    .Append(transform.DOScale(Vector3.zero, 0.5f))
                    .AppendCallback(Hide);
            }
            
        }
    }
}