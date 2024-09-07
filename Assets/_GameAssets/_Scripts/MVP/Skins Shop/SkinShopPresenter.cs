using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Factories.Skins;
using TapAndRun.Interfaces;
using TapAndRun.MVP.CharacterCamera.Model;
using TapAndRun.MVP.Skins_Shop.Model;
using TapAndRun.MVP.Skins_Shop.Views;
using TapAndRun.UI;
using UnityEngine;

namespace TapAndRun.MVP.Skins_Shop
{
    public class SkinShopPresenter : IInitializableAsync, IDecomposable
    {
        private readonly ISelfSkinShopModel _selfModel;
        private readonly ICameraModel _cameraModel;
        private readonly ISkinFactory _skinFactory;
        private readonly SkinShopView _shopScreenView;
        private readonly SkinShopSliderView _sliderView;

        private CancellationTokenSource _cts;

        public SkinShopPresenter(ISelfSkinShopModel selfModel, ICameraModel cameraModel, 
            ISkinFactory skinFactory, SkinShopView shopScreenView, SkinShopSliderView sliderView)
        {
            _selfModel = selfModel;
            _cameraModel = cameraModel;
            _skinFactory = skinFactory;
            _shopScreenView = shopScreenView;
            _sliderView = sliderView;
        }

        public UniTask InitializeAsync(CancellationToken token)
        {
            _cts = new CancellationTokenSource();

            _selfModel.IsDisplaying.OnChanged += UpdateDisplaying;

            _shopScreenView.BackButton.onClick.AddListener(_selfModel.BackTrigger.Trigger);
            _shopScreenView.LeftButton.onClick.AddListener(ScrollLeft);
            _shopScreenView.RightButton.onClick.AddListener(ScrollRight);
            _shopScreenView.ShopButton.Button.onClick.AddListener(ProccesShopButtonClick);
            
            return UniTask.CompletedTask;
        }

        private void UpdateDisplaying(bool isDisplaying)
        {
            if (isDisplaying)
            {
                _shopScreenView.Show();
                _cameraModel.SetSpecialView(Vector2.zero, 0f, 3f);
                FillAssortment();
            }
            else
            {
                _shopScreenView.Hide();
                ClearAssortment();
            }
        }

        private void FillAssortment()
        {
            FillAssortmentAsync(_cts.Token).Forget();
            
            async UniTaskVoid FillAssortmentAsync(CancellationToken token)
            {
                _sliderView.SkinsList = await _skinFactory.CreateAllSkinsAsync(_sliderView.SliderContent, token);
                _sliderView.SkinsCount = _sliderView.SkinsList.Count;
                _sliderView.Align();

                await ChangeSelectedSkin(0, token); //TODO Прокручивать до выбранного скина

                _selfModel.IsAssortmentPrepeared = true;
            }
        }

        private void ScrollRight()
        {
            var index = _sliderView.CurrentSkinIndex + 1;

            ChangeSelectedSkin(index, _cts.Token).Forget();
        }

        private void ScrollLeft()
        {
            var index = _sliderView.CurrentSkinIndex - 1;

            ChangeSelectedSkin(index, _cts.Token).Forget();
        }

        private async UniTask ChangeSelectedSkin(int skinIndex, CancellationToken token)
        {
            _sliderView.CurrentSkinIndex = Mathf.Clamp(skinIndex, 0, _sliderView.SkinsCount - 1);
            _selfModel.CurrentSkinsData = _skinFactory.SkinsConfig.SkinsData[_sliderView.CurrentSkinIndex];

            await _sliderView.ScrollContentAsync(token);

            _shopScreenView.SkinName.text = _selfModel.CurrentSkinsData.Name;
            UpdateShopButton();
        }

        private void UpdateShopButton()
        {
            if (_selfModel.IsSkinSelected())
            {
                _shopScreenView.ShopButton.SetState(ShopButtonState.Selected);
            }
            else if (_selfModel.IsUnlocked())
            {
                _shopScreenView.ShopButton.SetState(ShopButtonState.Purchased);
            }
            else if(_selfModel.IsCanPurchase())
            {
                _shopScreenView.ShopButton.SetState(ShopButtonState.CanPurchase, _selfModel.CurrentSkinsData.Price);
            }
            else
            {
                _shopScreenView.ShopButton.SetState(ShopButtonState.CantPurchase, _selfModel.CurrentSkinsData.Price);
            }
        }

        private void ProccesShopButtonClick()
        {
            if (_shopScreenView.ShopButton.State == ShopButtonState.CantPurchase)
            {
                _shopScreenView.ShopButton.PlayFailAnim();
            }
            else if(_shopScreenView.ShopButton.State == ShopButtonState.CanPurchase)
            {
                _shopScreenView.ShopButton.PlayAcceptAnim(CancellationToken.None).Forget();
                if (_selfModel.TryBuyCurrentSkin())
                {
                    UpdateShopButton();
                }
            }
            else if (_shopScreenView.ShopButton.State == ShopButtonState.Purchased)
            {
                _shopScreenView.ShopButton.PlayAcceptAnim(CancellationToken.None).Forget();
                _selfModel.SelectCurrentSkin();
                UpdateShopButton();
            }
        }

        private void ClearAssortment()
        {
            _selfModel.IsAssortmentPrepeared = false;
            
            _sliderView.SkinsList.Clear();
            _skinFactory.ReleaseUnusedSkins();
        }

        public void Decompose()
        {
            _cts?.Cancel();
            _cts?.Dispose();

            _selfModel.IsDisplaying.OnChanged -= UpdateDisplaying;

            _shopScreenView.BackButton.onClick.RemoveListener(_selfModel.BackTrigger.Trigger);
            _shopScreenView.LeftButton.onClick.RemoveListener(ScrollLeft);
            _shopScreenView.RightButton.onClick.RemoveListener(ScrollRight);
            _shopScreenView.ShopButton.Button.onClick.RemoveListener(ProccesShopButtonClick);
        }
    }
}