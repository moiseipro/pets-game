using DG.Tweening;
using UnityEngine;

namespace UI.Views
{
    public class UIComboTimerView : UIView
    {
        [SerializeField] private RectTransform _comboTimeLine;
        private Sequence _sequence;

        public void UpdateComboTimer(float value)
        {
            _sequence.Kill();
            _sequence = DOTween.Sequence();

            Show();
            _sequence
                .Append(_comboTimeLine.DOScaleX(1, 0.2f))
                .SetEase(Ease.InOutBack)
                .Append(_comboTimeLine.DOScaleX(0, value-0.2f))
                .SetEase(Ease.Linear)
                .AppendCallback(Hide);

        }
    }
}