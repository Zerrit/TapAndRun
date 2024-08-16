using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TapAndRun.UI
{
    public class CustomToggle : MonoBehaviour, IPointerClickHandler
    {
        public event Action OnStatusChanged;

        [Header("Toggle Background")]
        [SerializeField] private Image _background;
        [SerializeField] private Color _backgroundOffColor;
        [SerializeField] private Color _backgroundOnColor;
        
        [Header("Toggle Handle")]
        [SerializeField] private Image _handle;
        [SerializeField] private Color _handleOffColor;
        [SerializeField] private Color _handleOnColor;
        [SerializeField] private float _handleOffXPos;
        [SerializeField] private float _handleOnXPos;
        
        [Header("Anim Duration")]
        [SerializeField] private float _switchTime;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnStatusChanged?.Invoke();
        }

        public void SetState(bool state)
        {
            if (state)
            {
                _background.color = _backgroundOnColor;
                _handle.color = _handleOnColor;

                var pos = new Vector2(_handleOnXPos, _handle.rectTransform.anchoredPosition.y);
                _handle.rectTransform.anchoredPosition = pos;
            }
            else
            {
                _background.color = _backgroundOffColor;
                _handle.color = _handleOffColor;

                var pos = new Vector2(_handleOffXPos, _handle.rectTransform.anchoredPosition.y);
                _handle.rectTransform.anchoredPosition = pos;
            }
        }

        public void Switch(bool newState)
        {
            if (newState)
            {
                TurnOn();
            }
            else
            {
                TurnOff();
            }
        }

        private void TurnOn()
        {
            SwitchAsync(_handleOnXPos, _backgroundOnColor, _handleOnColor, _switchTime, this.GetCancellationTokenOnDestroy()).Forget();
        }

        private void TurnOff()
        {
            SwitchAsync(_handleOffXPos, _backgroundOffColor, _handleOffColor, _switchTime, this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTaskVoid SwitchAsync(float targetPos,Color backgroud, Color handle, float duration, CancellationToken token)
        {
            var firstTask = _handle.rectTransform.DOAnchorPosX(targetPos, duration)
                .AwaitForComplete(TweenCancelBehaviour.CompleteAndCancelAwait, token);

            var secondTask = _background.DOColor(backgroud, duration)
                .AwaitForComplete(TweenCancelBehaviour.CompleteAndCancelAwait, token);

            var thirdTask = _handle.DOColor(handle, duration)
                .AwaitForComplete(TweenCancelBehaviour.CompleteAndCancelAwait, token);

            await UniTask.WhenAll(firstTask, secondTask, thirdTask);
        }
    }
}