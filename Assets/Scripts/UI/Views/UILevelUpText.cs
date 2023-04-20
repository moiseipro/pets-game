using System;
using DG.Tweening;
using UnityEngine;

namespace UI.Views
{
    public class UILevelUpText : MonoBehaviour
    {
        private RectTransform _rectTransform;

        private Sequence _sequence;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            Hide();
        }

        public void ShowLevelUp()
        {
            gameObject.SetActive(true);
            _rectTransform.localScale = Vector3.zero;
            _sequence = DOTween.Sequence();
            _sequence
                .Append(_rectTransform.DOScale(2f, 0.5f))
                .Append(_rectTransform.DOScale(1f, 3f))
                .Insert(0f, _rectTransform.DORotate(new Vector3(0f,0f, 180f), 0.5f))
                .Insert(0.5f,_rectTransform.DORotate(new Vector3(0f,0f, 360f), 0.5f))
                .Append(_rectTransform.DOScale(0f, 0.5f))
                .AppendCallback(Hide);
        }

        private void Hide()
        {
            _sequence.Kill();
            gameObject.SetActive(false);
        }
    }
}