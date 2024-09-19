using System;
using System.Threading;
using Assets.SimpleLocalization.Scripts;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TapAndRun.UI
{
    public class ShopButton : MonoBehaviour
    {
        [field:SerializeField] public ShopButtonState State { get; private set; }
        [field:SerializeField] public Button Button { get; private set; }

        [SerializeField] private Image _buttonImage;
        [SerializeField] private Text _buttonText;
        [SerializeField] private GameObject _crystalIcon;

        [Header("Colors")]
        [SerializeField] private Color _selectedColor;
        [SerializeField] private Color _selectColor;
        [SerializeField] private Color _availableColor;
        [SerializeField] private Color _unavailableColor;

        [Header("Localization")]
        [SerializeField] private string _selectedLangKey;
        [SerializeField] private string _selectLangKey;

        [Header("Fail Animation")]
        [SerializeField] private Vector2 _failAnimVelocity = Vector2.right;
        [SerializeField] private float _failAnimDuration = .5f;

        [Header("Accept Animation")] 
        [SerializeField] private Vector3 _acceptAnimSize;
        [SerializeField] private float _acceptAnimDuration = .5f;
        [SerializeField] private Ease _acceptAnimCurve;

        private CancellationTokenSource _animCts;

        public void SetState(ShopButtonState state, int price = 0)
        {
            switch (state)
            {
                case ShopButtonState.Selected:
                {
                    SetSelectedState();
                    break;
                }

                case ShopButtonState.Purchased:
                {
                    SetPurchasedState();
                    break;
                }

                case ShopButtonState.CanPurchase:
                {
                    SetCanPurchaseState(price);
                    break;
                }

                case ShopButtonState.CantPurchase:
                {
                    SetCantPurchaseState(price);
                    break;
                }

                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private void SetSelectedState()
        {
            State = ShopButtonState.Selected;

            Button.interactable = false;
            _crystalIcon.SetActive(false);

            _buttonImage.color = _selectedColor;
            _buttonText.text = LocalizationManager.Localize(_selectedLangKey);
        }

        private void SetPurchasedState()
        {
            State = ShopButtonState.Purchased;

            Button.interactable = true;
            _crystalIcon.SetActive(false);

            _buttonImage.color = _selectColor;
            _buttonText.text = LocalizationManager.Localize(_selectLangKey);
        }

        private void SetCanPurchaseState(int price)
        {
            State = ShopButtonState.CanPurchase;

            Button.interactable = true;
            _crystalIcon.SetActive(true);

            _buttonText.text = price.ToString();
            _buttonImage.color = _availableColor;
        }        

        private void SetCantPurchaseState(int price)
        {
            State = ShopButtonState.CantPurchase;

            Button.interactable = true;
            _crystalIcon.SetActive(true);

            _buttonText.text = price.ToString();
            _buttonImage.color = _unavailableColor;
        }

        public void PlayFailAnim()
        {
            ResetToken();
            PlayFailAnimAsync(_animCts.Token).Forget();

            async UniTaskVoid PlayFailAnimAsync(CancellationToken token)
            {
                await transform.DOPunchPosition(_failAnimVelocity, _failAnimDuration)
                    .SetEase(Ease.InBounce)
                    .AwaitForComplete(TweenCancelBehaviour.CompleteAndCancelAwait, token);
            }
        }

        public async UniTask PlayAcceptAnim(CancellationToken token)
        {
            await transform.DOScale(_acceptAnimSize, _acceptAnimDuration)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(_acceptAnimCurve)
                .AwaitForComplete(TweenCancelBehaviour.CompleteAndCancelAwait, token);
        }

        private void ResetToken()
        {
            _animCts?.Cancel();
            _animCts?.Dispose();
            _animCts = new CancellationTokenSource();
        }
    }
}