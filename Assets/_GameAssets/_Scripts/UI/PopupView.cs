using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace TapAndRun.UI
{
    public class PopupView : MonoBehaviour
    {
        [field:SerializeField] public CanvasGroup Fade { get; private set; }
        [field:SerializeField] public RectTransform Content { get; private set; }

        [Header("Fade")]
        [SerializeField] private float _fadeInDuration;
        [SerializeField] private float _fadeOutDuration;

        [Header("Parent")]
        [SerializeField] private float _moveInDuration;
        [SerializeField] private float _moveOutDuration;

        [Header("Parent Anim")]
        [SerializeField] private Ease _animType;
        [SerializeField, Range(-1, 1)] private int _xAnimDirection;
        [SerializeField, Range(-1, 1)] private int _yAnimDirection;

        private Vector2 HideAnimPosition => new((Screen.width + Content.rect.width) * _xAnimDirection, (Screen.height + Content.rect.height) * _yAnimDirection);

        public void Show()
        {
            ShowAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        public async UniTask ShowAsync(CancellationToken token)
        {
            Fade.gameObject.SetActive(true);
            Fade.DOFade(1f, _fadeInDuration)
                .AwaitForComplete(TweenCancelBehaviour.CompleteAndCancelAwait, token).Forget();

            Content.gameObject.SetActive(true);
            await Content.DOLocalMove(Vector3.zero, _moveInDuration)
                .SetEase(_animType)
                .From(HideAnimPosition)
                .AwaitForComplete(TweenCancelBehaviour.CompleteAndCancelAwait, token);
        }

        public void Hide()
        {
            HideAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        public async UniTask HideAsync(CancellationToken token)
        {
            Fade.DOFade(0f, _fadeOutDuration)
                .AwaitForComplete(TweenCancelBehaviour.CompleteAndCancelAwait, token).Forget();

            await Content.DOLocalMove(HideAnimPosition, _moveOutDuration)
                .SetEase(_animType)
                .AwaitForComplete(TweenCancelBehaviour.CompleteAndCancelAwait, token);

            Fade.gameObject.SetActive(false);
            Content.gameObject.SetActive(false);
        }
    }
}