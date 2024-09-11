using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Configs;
using TapAndRun.Interfaces;
using TapAndRun.MVP.Character.Model;
using TapAndRun.Services.Update;
using TapAndRun.Tools.Reactivity;
using UnityEngine;

namespace TapAndRun.MVP.CharacterCamera.Model
{
    public class CameraModel : ISelfCameraModel, ICameraModel, ILateUpdatable, IDecomposable
    {
        public ReactiveProperty<Vector3> Position { get; private set; }
        public ReactiveProperty<float> Rotation { get; private set; }
        public ReactiveProperty<float> Height { get; private set; }

        public int Difficulty { get; private set; }

        private int _maxDifficultyLevel;
        private bool _isFollowActive;

        private CancellationTokenSource _cts;

        private const int MinDifficultyLevel = 1;

        private readonly CameraConfig _config;
        private readonly ILateUpdateService _updateService;
        private readonly ICharacterModel _characterModel;

        public CameraModel(CameraConfig config, ILateUpdateService updateService, ICharacterModel characterModel)
        {
            _config = config;
            _updateService = updateService;
            _characterModel = characterModel;
        }

        public UniTask InitializeAsync(CancellationToken token)
        {
            _cts = new CancellationTokenSource();
            Position = new ReactiveProperty<Vector3>();
            Rotation = new ReactiveProperty<float>();
            Height = new ReactiveProperty<float>(_config.Height + _config.HeightStep);

            _maxDifficultyLevel = _config.RotationDifficulties.Length;

            _updateService.Subscribe(this);

            return UniTask.CompletedTask;
        }

        public void LateUpdate()
        {
            FollowCharacter();
        }

        public void ChangeDifficulty(int newDifficulty)
        {
            _isFollowActive = true;

            Difficulty = Mathf.Clamp(newDifficulty, MinDifficultyLevel, _maxDifficultyLevel);
            var targetHeight = Difficulty * _config.HeightStep + _config.Height;

            ChangeDistanceAsync(targetHeight).Forget();
        }

        public void SetSpecialView(Vector3 position, float rotation, float height)
        {
            _isFollowActive = false;
            Position.Value = position + _config.Offset;
            Rotation.Value = rotation;
            ChangeDistanceAsync(height).Forget();
        }

        public void SetRotation(float rotation = 0)
        {
            Rotation.Value = rotation;
        }

        public async UniTaskVoid FlyUpAsync()
        {
            await ChangeDistanceAsync(_config.LoseHeight);
        }

        public async UniTaskVoid TurnAsync(int direction)
        {
            var originRotation = Rotation.Value;
            var angle = _config.RotationDifficulties[Difficulty - 1] * direction;

            float t = 0;

            while (t < 1)
            {
                t += _config.TurnSpeed * Time.deltaTime;
                Rotation.Value = Mathf.LerpAngle(originRotation, originRotation + angle, t);

                await UniTask.NextFrame(_cts.Token);
            }
        }

        public async UniTask ChangeDistanceAsync(float targetHaight)
        {
            var originDistance = Height.Value;
            float t = 0;

            while (t < 1)
            {
                t += 3 * Time.deltaTime;
                Height.Value = Mathf.Lerp(originDistance, targetHaight, t);

                await UniTask.NextFrame(_cts.Token);
            }
        }

        private void FollowCharacter()
        {
            if (!_isFollowActive)
            {
                return;
            }

            var targetPosition = _characterModel.Position.Value + _config.Offset;
            Position.Value = targetPosition;
        }

        public void Decompose()
        {
            _cts?.Cancel();
            _cts?.Dispose();

            _updateService.Unsubscribe(this);
        }
    }
}