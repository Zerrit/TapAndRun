using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

namespace TapAndRun.UI
{
    public class PopupView : MonoBehaviour
    {
        [field:SerializeField] public CanvasGroup Fade { get; private set; }
        [field:SerializeField] public Transform Parent { get; private set; }
        
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


        private Vector2 HideAnimPosition => new(Screen.width * 2 * _xAnimDirection, Screen.height * 2 * _yAnimDirection);

        public void Show()
        {
            ShowAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        public async UniTask ShowAsync(CancellationToken token)
        {
            Fade.gameObject.SetActive(true);
            Fade.DOFade(1f, 0.1f)
                .AwaitForComplete(TweenCancelBehaviour.CompleteAndCancelAwait, token);

            Parent.gameObject.SetActive(true);
            await Parent.DOLocalMove(Vector3.zero, _moveInDuration)
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
            await Parent.DOLocalMove(HideAnimPosition, _moveOutDuration)
                .SetEase(_animType)
                .AwaitForComplete(TweenCancelBehaviour.CompleteAndCancelAwait, token);

            await Fade.DOFade(0f, 0.1f)
                .AwaitForComplete(TweenCancelBehaviour.CompleteAndCancelAwait, token);

            Fade.gameObject.SetActive(false);
            Parent.gameObject.SetActive(false);
        }
    }
}