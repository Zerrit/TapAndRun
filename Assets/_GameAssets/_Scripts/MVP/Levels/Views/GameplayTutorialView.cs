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

        public void PlayTapAnimAsync()
        {
            _cursor.transform.DOScale(new Vector3(0.8f, 0.8f, 1f), 0.5f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine)
                .AwaitForComplete(TweenCancelBehaviour.KillAndCancelAwait, destroyCancellationToken);
        }

        public override void Show()
        {
            base.Show();

            PlayTapAnimAsync();
        }

        public override void Hide()
        {
            base.Hide();
        }
    }
}