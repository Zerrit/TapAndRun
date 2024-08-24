using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Interfaces;
using TapAndRun.MVP.Character.Model;
using TapAndRun.MVP.Character.View;

namespace TapAndRun.MVP.Character
{
    public class CharacterPresenter : IInitializableAsync
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
            _model.AnimMultiplier.OnChanged += _view.UpdateAnimMultiplier;

            _model.OnBeganIdle += DisplayIdle;
            _model.OnBeganRunning += DisplayRunning;
            _model.OnBeganTurning += DisplayTurnning;
            _model.OnBeganJumping += DisplayJumping;
            _model.OnFalled += DisplayFalling; 
            
            return UniTask.CompletedTask;
        }

        private void DisplayIdle()
        {
            _view.ActivateAnimation(_view.Idle);
        }
        
        private void DisplayRunning()
        {
            _view.ActivateAnimation(_view.Run);
        }

        private void DisplayJumping()
        {
            _view.ActivateAnimation(_view.Jump);
        }

        private void DisplayFalling()
        {
            _view.ActivateAnimation(_view.Fall);
        }

        private void DisplayTurnning()
        {

        }
    }
}