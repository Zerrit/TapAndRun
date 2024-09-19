using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Factories.Skins;
using TapAndRun.Interfaces;
using TapAndRun.MVP.Character.Model;
using TapAndRun.MVP.Character.View;
using TapAndRun.Services.Audio;

namespace TapAndRun.MVP.Character
{
    public class CharacterPresenter : IInitializableAsync, IDecomposable
    {
        private CancellationTokenSource _cts;

        private readonly ISelfCharacterModel _model;
        private readonly ISkinFactory _skinFactory;
        private readonly IAudioService _audioService;
        private readonly CharacterView _view;

        public CharacterPresenter(ISelfCharacterModel model, ISkinFactory skinFactory, IAudioService audioService, CharacterView view)
        {
            _model = model;
            _skinFactory = skinFactory;
            _audioService = audioService;
            _view = view;
        }

        public async UniTask InitializeAsync(CancellationToken token)
        {
            _cts = new CancellationTokenSource();

            await SetSkinAsync(_model.SelectedSkin.Value);

            _model.SelectedSkin.Subscribe(SetSkin);
            _model.IsActive.Subscribe(_view.gameObject.SetActive);
            _model.Position.Subscribe(_view.UpdatePosition);
            _model.Rotation.Subscribe(_view.UpdateRotation);
            _model.AnimMultiplier.Subscribe(_view.UpdateAnimMultiplier);
            _model.SfxAcceleration.Subscribe(_view.UpdateSfxAcceleration);
            _model.IsMoving.Subscribe(_view.UpdateMoving);
            _model.IsFall.Subscribe(_view.UpdateFalling);

            _model.OnBeganTurning += _view.DisplayTurning;
            _model.OnBeganJumping += _view.DisplayJumping;
        }

        private void SetSkin(string skinId)
        {
            SetSkinAsync(skinId).Forget();
        }

        private async UniTask SetSkinAsync(string skinId)
        {
            var skin = await _skinFactory.ChangeSkinTo(skinId, _view.SkinHandler, _cts.Token);

            _view.InitSkin(skin);

            _view.Sfx.SetMixer(_audioService.MixerGroup);
            _view.UpdateAnimMultiplier(_model.AnimMultiplier.Value);
        }

        public void Decompose()
        {
            _cts?.Cancel();
            _cts?.Dispose();

            _model.SelectedSkin.Unsubscribe(SetSkin);
            _model.IsActive.Unsubscribe(_view.gameObject.SetActive);
            _model.Position.Unsubscribe(_view.UpdatePosition);
            _model.Rotation.Unsubscribe(_view.UpdateRotation);
            _model.AnimMultiplier.Unsubscribe(_view.UpdateAnimMultiplier);
            _model.SfxAcceleration.Unsubscribe(_view.UpdateSfxAcceleration);
            _model.IsMoving.Unsubscribe(_view.UpdateMoving);
            _model.IsFall.Unsubscribe(_view.UpdateFalling);

            _model.OnBeganTurning -= _view.DisplayTurning;
            _model.OnBeganJumping -= _view.DisplayJumping;
        }
    }
}