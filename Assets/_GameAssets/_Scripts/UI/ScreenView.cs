using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace TapAndRun.UI
{
    public abstract class ScreenView : MonoBehaviour
    {
        [field:SerializeField] public CanvasGroup Content { get; private set; }

        [SerializeField] private float _fadeOutDuration;
        [SerializeField] private float _fadeInDuration;
        
        public virtual void Show()
        {
            ShowAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        public async UniTask ShowAsync(CancellationToken token)
        {
            Content.alpha = 0;
            Content.gameObject.SetActive(true);
            
            await Content.DOFade(1f, _fadeInDuration)
                .AwaitForComplete(TweenCancelBehaviour.CompleteAndCancelAwait, token);
        }
        
        public virtual void Hide()
        {
            HideAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }
        
        public async UniTask HideAsync(CancellationToken token)
        {
            Content.alpha = 1;

            await Content.DOFade(0f, _fadeOutDuration)
                .AwaitForComplete(TweenCancelBehaviour.CompleteAndCancelAwait, token);

            Content.gameObject.SetActive(false);
        }
    }
}