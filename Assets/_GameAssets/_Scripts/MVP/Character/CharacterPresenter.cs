using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Interfaces;
using TapAndRun.MVP.Character.Model;
using TapAndRun.MVP.Character.View;

namespace TapAndRun.MVP.Character
{
    public class CharacterPresenter : IInitializableAsync, IDisposable
    {
        private readonly ISelfCharacterModel _model;
        private readonly CharacterView _view;

        public CharacterPresenter(ISelfCharacterModel model, CharacterView view)
        {
            _model = model;
            _view = view;
        }

        public UniTask InitializeAsync(CancellationToken token)
        {
            _model.Position.OnChanged += _view.UpdatePosition;
            _model.Rotation.OnChanged += _view.UpdateRotation;
            _model.IsMoving.OnChanged += _view.UpdateMoving;
            _model.IsFall.OnChanged += _view.UpdateFalling;
            _model.AnimMultiplier.OnChanged += _view.UpdateAnimMultiplier;

            _model.OnBeganTurning += DisplayTurnning;
            _model.OnBeganJumping += DisplayJumping;
            _model.OnFinishedJumping += _view.Sfx.PlayRunSfx;

            return UniTask.CompletedTask;
        }

        private void DisplayJumping()
        {
            _view.ActivateAnimation(_view.Jump);
            _view.Sfx.StopRunSfx();
            _view.Sfx.PlayJumpSfx();
        }

        private void DisplayTurnning()
        {
            _view.Sfx.PlayTurnSfx();
        }

        public void Dispose()
        {
            _model.Position.OnChanged -= _view.UpdatePosition;
            _model.Rotation.OnChanged -= _view.UpdateRotation;
            _model.IsMoving.OnChanged -= _view.UpdateMoving;
            _model.IsFall.OnChanged -= _view.UpdateFalling;
            _model.AnimMultiplier.OnChanged -= _view.UpdateAnimMultiplier;

            _model.OnBeganTurning -= DisplayTurnning;
            _model.OnBeganJumping -= DisplayJumping;
        }
    }
}