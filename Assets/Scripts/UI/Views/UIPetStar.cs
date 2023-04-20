using System;
using DG.Tweening;
using UnityEngine;

namespace UI.Views
{
    public class UIPetStar : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private Sequence _sequence;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void LoadLevel()
        {
            _sequence.Kill();
            _sequence = DOTween.Sequence();
            
            _rectTransform.localScale = Vector3.zero;

            _sequence
                .Append(_rectTransform.DOScale(1f, 1f))
                .SetEase(Ease.InOutBack);
        }

        public void LoadLevel(int order)
        {
            _sequence.Kill();
            _sequence = DOTween.Sequence();
            
            _rectTransform.localScale = Vector3.zero;
            
            _sequence
                .SetDelay(order*0.1f)
                .Append(_rectTransform.DOScale(3f, 2f))
                .Append(_rectTransform.DOScale(1f, 0.5f))
                .SetEase(Ease.InOutBack);
        }

        public void Death()
        {
            _sequence.Kill();
            Destroy(gameObject);
        }

        private void OnEnable()
        {
            _rectTransform = GetComponent<RectTransform>();
        }
    }
}