using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Configs;
using TapAndRun.Factories.Skins;
using TapAndRun.Interfaces;
using TapAndRun.MVP.Character.Model;
using TapAndRun.MVP.Skins_Shop.Model;
using TapAndRun.MVP.Skins_Shop.Views;
using TapAndRun.MVP.Wallet.Model;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TapAndRun.MVP.Skins_Shop
{
    public class SkinShopPresenter : IInitializableAsync, IDisposable
    {
        private readonly ISelfSkinShopModel _model;
        private readonly IWalletModel _walletModel;
        private readonly ICharacterModel _characterModel;
        private readonly ISkinFactory _skinFactory;
        private readonly SkinShopView _view;
        private readonly SkinShopSliderView _sliderView;

        private SkinData _currentSkinsData;

        private CancellationTokenSource _cts;

        public SkinShopPresenter(ISelfSkinShopModel model, IWalletModel walletModel, ICharacterModel characterModel, 
            SkinShopView view, ISkinFactory skinFactory, SkinShopSliderView sliderView)
        {
            _model = model;
            _walletModel = walletModel;
            _characterModel = characterModel;
            _view = view;
            _skinFactory = skinFactory;
            _sliderView = sliderView;
        }

        public UniTask InitializeAsync(CancellationToken token)
        {
            _cts = new CancellationTokenSource();

            _model.IsDisplaying.OnChanged += UpdateDisplaying;

            _view.BackButton.onClick.AddListener(_model.BackTrigger.Trigger);
            _view.LeftButton.onClick.AddListener(ScrollLeft);
            _view.RightButton.onClick.AddListener(ScrollRight);
            
            return UniTask.CompletedTask;
        }

        private void UpdateDisplaying(bool isDisplaying)
        {
            if (isDisplaying)
            {
                _view.Show();
                FillAssortment();
            }
            else
            {
                _view.Hide();
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
                await ScrollToAsync(0, token);

                _model.IsAssortmentPrepeared = true;
            }
        }

        private void ClearAssortment()
        {
            _sliderView.SkinsList.Clear();
            _skinFactory.DisposeUnusedSkins();
        }

        private void ScrollRight()
        {
            ScrollToAsync(++_sliderView.CurrentSkinIndex, _cts.Token).Forget();
        }

        private void ScrollLeft()
        {
            ScrollToAsync(--_sliderView.CurrentSkinIndex, _cts.Token).Forget();
        }
        
        private async UniTask ScrollToAsync(int newIndex, CancellationToken token)
        {
            await _sliderView.ScrollAsync(newIndex, token);

            _currentSkinsData = _skinFactory.SkinsConfig.SkinsData[newIndex];

            _sliderView.CurrentSkinIndex = newIndex;
            _view.SkinName.text = _currentSkinsData.Name;
            UpdateShopButton();
        }

        private void UpdateShopButton()
        {
            if (_characterModel.SelectedSkin.Value.Equals(_currentSkinsData.Name))
            {
                _view.ShopButton.SetSelectedMode();
            }
            else if (_model.UnlockedSkins.Contains(_currentSkinsData.Name))
            {
                _view.ShopButton.SetSelectMode();
            }
            else
            {
                _view.ShopButton.SetBuyMode(_currentSkinsData.Price, _walletModel.IsEnough(_currentSkinsData.Price));
            }
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}