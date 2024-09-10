using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TapAndRun.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TapAndRun.MVP.Wallet.View
{
    public class WalletTutorialView : ScreenView
    {
        [SerializeField] private Image _cursor;
        [SerializeField] private RectTransform _animTarget;
        [SerializeField] private Vector3 _targetffset;
        [SerializeField] private Vector3 _cursorOffset;

        private CancellationTokenSource _cts = new();

        public void PlayCursorAnim()
        {
            var target = _animTarget.localPosition + _targetffset;
            _cursor.transform.localPosition = target + _cursorOffset;

            _cursor.transform.DOLocalMove(target, 0.5f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine)
                .AwaitForComplete(TweenCancelBehaviour.KillAndCancelAwait, _cts.Token);
        }

        public override void Show()
        {
            base.Show();

            PlayCursorAnim();
        }

        public override void Hide()
        {
            base.Hide();

            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}