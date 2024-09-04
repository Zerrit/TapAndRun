using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Configs;
using TapAndRun.Interfaces;
using TapAndRun.MVP.Character.Model;
using TapAndRun.Services.Update;
using TapAndRun.Tools.Reactivity;
using UnityEngine;

namespace TapAndRun.MVP.CharacterCamera
{
    public class CameraModel : ISelfCameraModel, ICameraModel, ILateUpdatable, IDisposable
    {
        public ReactiveProperty<Vector3> Position { get; private set; }
        public ReactiveProperty<float> Rotation { get; private set; }
        public ReactiveProperty<float> Height { get; private set; }

        public int Difficulty { get; private set; } //??

        private int _maxDifficultyLevel;
        private int _minDifficultyLevel = 1;
        private bool _isFollowActive;

        private CancellationTokenSource _cts;

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
            _isFollowActive = true;

            _updateService.Subscribe(this);

            return UniTask.CompletedTask;
        }

        public void LateUpdate()
        {
            FollowCharacter();
        }

        public void ChangeDifficulty(int newDifficulty)
        {
            Difficulty = Mathf.Clamp(newDifficulty, _minDifficultyLevel, _maxDifficultyLevel);
            var targetHeight = Difficulty * _config.HeightStep;

            ChangeDistanceAsync(targetHeight).Forget();
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

        /*public async UniTaskVoid TurnAsync(float angle)
        {
            var originRotation = Rotation.Value;
            float t = 0;

            while (t < 1)
            {
                t += _config.TurnSpeed * Time.deltaTime;
                Rotation.Value = Mathf.LerpAngle(originRotation, originRotation + angle, t);

                await UniTask.NextFrame(_cts.Token);
            }
        }*/

        public async UniTask ChangeDistanceAsync(float target)
        {
            var originDistance = Height.Value;
            float t = 0;

            while (t < 1)
            {
                t += 3 * Time.deltaTime;
                Height.Value = Mathf.Lerp(originDistance, _config.Height + target, t);

                await UniTask.NextFrame(_cts.Token);
            }
        }

        private void FollowCharacter()
        {
            if (!_isFollowActive)
            {
                return;
            }

            Vector3 targetPosition = _characterModel.Position.Value + _config.Offset;
            Position.Value = targetPosition;
        }

        public void Dispose()
        {
            _updateService.Unsubscribe(this);

            _cts.Cancel();
            _cts.Dispose();
        }
    }
}