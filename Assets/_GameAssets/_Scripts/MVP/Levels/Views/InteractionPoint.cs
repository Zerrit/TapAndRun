using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.TapSystem;
using UnityEngine;

namespace TapAndRun.MVP.Levels.Views
{
    public class InteractionPoint : MonoBehaviour
    {
        [field:SerializeField] public InteractType CommandType { get; private set; }

        [field:SerializeField] public SpriteRenderer Icon { get; private set; }

        public void Activate()
        {
            FadeAsync(1f, 0f, this.GetCancellationTokenOnDestroy()).Forget();
        }

        public void Deactivate()
        {
            FadeAsync(0f, 0.2f, this.GetCancellationTokenOnDestroy()).Forget();
        }

        public void SetDefault()
        {
            FadeAsync(0.3f, 0f, this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTask FadeAsync(float value, float duration, CancellationToken token)
        {
            await Icon.DOFade(value, duration)
                .AwaitForComplete(TweenCancelBehaviour.CompleteAndCancelAwait, token);
        }

        /*private void SetTransparent(float alpha)
        {
            Icon.color = new Color(Icon.color.r, Icon.color.g, Icon.color.b, alpha);
        }*/
    }
}
