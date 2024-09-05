using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Factories.Skins;
using TapAndRun.Interfaces;
using TapAndRun.MVP.Character.Model;
using TapAndRun.MVP.Character.View;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TapAndRun.MVP.Character
{
    public class CharacterPresenter : IInitializableAsync, IDisposable
    {
        private readonly ISelfCharacterModel _model;
        private readonly ISkinFactory _skinFactory;
        private readonly CharacterView _view;

        private CancellationTokenSource _cts;

        public CharacterPresenter(ISelfCharacterModel model, ISkinFactory skinFactory, CharacterView view)
        {
            _model = model;
            _skinFactory = skinFactory;
            _view = view;
        }

        public async UniTask InitializeAsync(CancellationToken token)
        {
            _cts = new CancellationTokenSource();
            
            await SetSkinAsync(_model.SelectedSkin.Value);
            //_model.SelectedSkin.Subscribe(SetSkin, true);
            _model.IsActive.OnChanged += _view.gameObject.SetActive;
            _model.Position.OnChanged += _view.UpdatePosition;
            _model.Rotation.OnChanged += _view.UpdateRotation;
            _model.IsMoving.OnChanged += _view.UpdateMoving;
            _model.IsFall.OnChanged += _view.UpdateFalling;
            _model.AnimMultiplier.OnChanged += _view.UpdateAnimMultiplier;

            _model.OnBeganTurning += _view.DisplayTurning;
            _model.OnBeganJumping += _view.DisplayJumping;
            _model.OnFinishedJumping += _view.Sfx.PlayRunSfx;
        }

        private async UniTask SetSkinAsync(string skinId)
        {
            var skin = await _skinFactory.ChangeSkinTo(skinId, _view.SkinHandler, _cts.Token);

            _view.InitSkin(skin);
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            
            _model.IsActive.OnChanged -= _view.gameObject.SetActive;
            _model.Position.OnChanged -= _view.UpdatePosition;
            _model.Rotation.OnChanged -= _view.UpdateRotation;
            _model.IsMoving.OnChanged -= _view.UpdateMoving;
            _model.IsFall.OnChanged -= _view.UpdateFalling;
            _model.AnimMultiplier.OnChanged -= _view.UpdateAnimMultiplier;

            _model.OnBeganTurning -= _view.DisplayTurning;
            _model.OnBeganJumping -= _view.DisplayJumping;
            _model.OnFinishedJumping -= _view.Sfx.PlayRunSfx;
        }
    }
}