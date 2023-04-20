using DG.Tweening;
using UnityEngine;

namespace UI.Views
{
    public class UISatietyView : UIView
    {
        [SerializeField] private RectTransform _satietyLine;
        private Sequence _sequence;
        
        public void UpdateSatietyBar(float value)
        {
            _sequence.Kill();
            _sequence = DOTween.Sequence();

            Show();
            _satietyLine.localScale = new Vector3(1f * value, 1f, 1f);
            // _sequence
            //     .Append(_satietyLine.DOScaleX(value, 0.1f))
            //     .SetEase(Ease.InOutBack);
        }
    }
}