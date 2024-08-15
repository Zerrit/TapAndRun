using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace TapAndRun.UI
{
    public abstract class ScreenView : MonoBehaviour
    {
        [field:SerializeField] public CanvasGroup Parent { get; private set; }

        [SerializeField] private float _fadeOutDuration;
        [SerializeField] private float _fadeInDuration;
        
        public virtual void Show()
        {
            ShowAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        public async UniTask ShowAsync(CancellationToken token)
        {
            Parent.alpha = 0;
            Parent.gameObject.SetActive(true);
            
            await Parent.DOFade(1f, 0.3f)
                .AwaitForComplete(TweenCancelBehaviour.CompleteAndCancelAwait, token);
        }
        
        public virtual void Hide()
        {
            HideAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }
        
        public async UniTask HideAsync(CancellationToken token)
        {
            Parent.alpha = 1;

            await Parent.DOFade(1f, 0.3f)
                .AwaitForComplete(TweenCancelBehaviour.CompleteAndCancelAwait, token);

            Parent.gameObject.SetActive(false);
        }
    }
}