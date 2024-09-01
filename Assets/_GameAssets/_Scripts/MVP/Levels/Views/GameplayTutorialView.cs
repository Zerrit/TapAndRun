using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TapAndRun.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TapAndRun.MVP.Levels.Views
{
    public class GameplayTutorialView : ScreenView
    {
        [SerializeField] private Image _cursor;

        private CancellationTokenSource _cts;

        public void PlayTapAnimAsync()
        {
            _cts = new CancellationTokenSource();
            
            _cursor.transform.DOScale(new Vector3(0.8f, 0.8f, 1f), 0.5f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine)
                .AwaitForComplete(TweenCancelBehaviour.KillAndCancelAwait, _cts.Token);
        }

        public override void Show()
        {
            base.Show();

            PlayTapAnimAsync();
        }

        public override void Hide()
        {
            base.Hide();

            _cts?.Cancel();
            _cts?.Dispose();
            Debug.Log("Проверить останавливается ли анимация");
        }
    }
}