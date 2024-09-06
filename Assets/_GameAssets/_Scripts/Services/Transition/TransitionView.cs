using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace TapAndRun.Services.Transition
{
    public class TransitionView : MonoBehaviour, ITransitionView
    {
        [SerializeField] private RectTransform _curtain;

        [SerializeField] private float _inDuration;
        [SerializeField] private Ease _inCurve;
        
        [SerializeField] private float _outDuration;
        [SerializeField] private Ease _outCurve;

        public async UniTask ShowAsync(CancellationToken token)
        {
            var startPos =  (_curtain.rect.height / 2 + Screen.height) * -1;
            _curtain.gameObject.SetActive(true);

            await _curtain.DOLocalMoveY(0, _inDuration)
                .From(startPos)
                .SetEase(_inCurve)
                .AwaitForComplete(TweenCancelBehaviour.CompleteAndCancelAwait, token);
        }

        public async UniTask HideAsync(CancellationToken token)
        {
            var targetPos =  (_curtain.rect.height / 2 + Screen.height);
            
            await _curtain.DOLocalMoveY(targetPos, _outDuration)
                .SetEase(_outCurve)
                .AwaitForComplete(TweenCancelBehaviour.CompleteAndCancelAwait, token);
            
            _curtain.gameObject.SetActive(false);
        }
    }
}