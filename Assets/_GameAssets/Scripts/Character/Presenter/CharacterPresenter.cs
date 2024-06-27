using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Character.Model;
using TapAndRun.Character.View;
using UnityEngine;
using VContainer.Unity;

namespace TapAndRun.Character.Presenter
{
    public class CharacterPresenter : IAsyncStartable
    {
        private float _turnSpeed = 12f; // Вынести в конфиг
        private float _centeringSpeed = 7f; // Вынести в конфиг
        
        
        private CharacterModel _model;
        private CharacterView _view;
        
        public CharacterPresenter(CharacterModel model, CharacterView view)
        {
            _model = model;
            _view = view;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await UniTask.CompletedTask;
        }

        private async UniTask CenteringAsync(Vector3 centre)
        {
            _model.IsActive = false;

            var origin = _view.CharacterTransform.position;
            var t = 0f;

            while (t < 1f)
            {
                t += _centeringSpeed * Time.deltaTime;

                if (IsPayerDirectionVecrtical())
                {
                    _view.CharacterTransform.position = new Vector2(Mathf.Lerp(origin.x, centre.x, t), _view.CharacterTransform.position.y);
                }
                else
                {
                    _view.CharacterTransform.position = new Vector2(_view.CharacterTransform.position.x, Mathf.Lerp(origin.y, centre.y, t));
                }

                UniTask.Yield();
            }

            await UniTask.CompletedTask;
        }
        
        private async UniTask TurnAsync(float targetAngle)
        {
            var t = 0f;

            while (t < 1f)
            {
                t += _turnSpeed * Time.deltaTime;
                var originAngle = _view.CharacterTransform.eulerAngles.z;
                var angle = Mathf.LerpAngle(originAngle, originAngle + targetAngle, t);

                _view.CharacterTransform.eulerAngles = new Vector3(0, 0, angle);

                UniTask.Yield();
            }

            await UniTask.CompletedTask;
        }

        private async UniTask JumpSync()
        {
            _model.IsActive = false;
            _view.ActiveJumpAnimation();

            var t = Time.time;

            while ((t+0.6f - (0.1f * 3)) > Time.time) //TODO добавить логику согласно изменению сложности
            {
                UniTask.Yield();
            }

            _view.ActiveRunAnimation();
            _model.IsActive = true;

            await UniTask.CompletedTask;
        }

        private bool IsPayerDirectionVecrtical()
        {
            var rotation = _view.CharacterTransform.eulerAngles.z;

            return ((Mathf.Abs(rotation) < 5f) || (Mathf.Abs(rotation) > 355f) ||
                    (Mathf.Abs(rotation) > 175f && Mathf.Abs(rotation) < 185f));
        }
    }
}
