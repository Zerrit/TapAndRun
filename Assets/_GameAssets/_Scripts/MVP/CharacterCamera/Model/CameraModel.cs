using System.Collections.Generic;
using System.Linq;
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
    public class CameraModel : ISelfCameraModel, ICameraModel, ICameraZoom, ILateUpdatable, IDecomposable
    {
        public ReactiveProperty<Vector3> Position { get; private set; }
        public ReactiveProperty<float> Rotation { get; private set; }
        public ReactiveProperty<float> Height { get; private set; }

        public CameraMode CurrentMode { get; private set; }

        private CancellationTokenSource _cts;

        private Dictionary<CameraMode, int> _cameraModes = new ();

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
            _cameraModes = _config.CameraModes.ToDictionary(x => x.Mode, x => x.Angle);

            _updateService.Subscribe(this);

            return UniTask.CompletedTask;
        }

        public void LateUpdate()
        {
            FollowCharacter();
        }

        public void ChangeMode(int characterSpeedLevel, CameraMode mode)
        {
            var targetHeight = characterSpeedLevel * _config.HeightStep + _config.Height;
            CurrentMode = mode;
            
            ChangeDistanceAsync(targetHeight).Forget();
        }

        public void SetRotation(float rotation = 0)
        {
            Rotation.Value = rotation;
        }

        public async UniTaskVoid TurnAsync(int direction)
        {
            var originRotation = Rotation.Value;
            var angle = _cameraModes[CurrentMode] * direction;

            if (CurrentMode == CameraMode.Random)
            {
                angle *= (Random.value > 0.5f) ? -1 : 1;
            }

            float t = 0;

            while (t < 1)
            {
                t += _config.TurnSpeed * Time.deltaTime;
                Rotation.Value = Mathf.LerpAngle(originRotation, originRotation + angle, t);

                await UniTask.NextFrame(_cts.Token);
            }
        }

        public async UniTaskVoid SetLoseZoomAsync()
        {
            await ChangeDistanceAsync(_config.LoseHeight);
        }

        public async UniTaskVoid SetFreeViewZoomAsync()
        {
            await ChangeDistanceAsync(_config.FreeViewHeight);
        }

        public async UniTaskVoid SetShopZoomAsync()
        {
            await ChangeDistanceAsync(_config.ShopHeight);
        }

        public async UniTask ChangeDistanceAsync(float targetHaight)
        {
            var originDistance = Height.Value;

            float x = 0;
            float t = 0;

            while (x < 1)
            {
                t += _config.ZoomSpeed * Time.deltaTime;
                x = 1 - Mathf.Pow(1 - t, 3);
                
                Height.Value = Mathf.Lerp(originDistance, targetHaight, x);

                await UniTask.NextFrame(_cts.Token);
            }
        }

        private void FollowCharacter()
        {
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