using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Configs;
using TapAndRun.Interfaces;
using TapAndRun.PlayerData;
using TapAndRun.Services.Update;
using TapAndRun.Tools.Reactivity;
using UnityEngine;

namespace TapAndRun.MVP.Character.Model
{
    public class CharacterModel : ISelfCharacterModel, ICharacterModel, IUpdatable, IProgressable, IDecomposable
    {
        public event Action OnBeganTurning;
        public event Action OnBeganJumping;

        public BoolReactiveProperty IsActive { get; private set; }

        public ReactiveProperty<Vector3> Position { get; private set; }
        public ReactiveProperty<float> Rotation { get; private set; }

        public ReactiveProperty<float> AnimMultiplier { get; private set; }
        public ReactiveProperty<float> SfxAcceleration { get; private set; }

        public BoolReactiveProperty IsMoving { get; private set; }
        public BoolReactiveProperty IsFall { get; private set; }

        public ReactiveProperty<string> SelectedSkin { get; private set; }

        public string SaveKey => "Character";

        private Vector3 _roadCheckerPosition;
        private float _currentSpeed;
        private bool _isVulnerable;
        private float _baseAnimSpeed;

        private CancellationTokenSource _cts;

        private const float JumpDuration = 1f;
        private const float AnimationTargetMoveSpeed = 2f;
        private const float SfxAccelerationStep = .1f; //TODO донастроить

        private static readonly Vector2 MoveDirection = Vector2.up;

        private readonly CharacterConfig _config;
        private readonly IUpdateService _updateService;

        public CharacterModel(CharacterConfig config, IUpdateService updateService)
        {
            _config = config;
            _updateService = updateService;
        }

        public UniTask InitializeAsync(CancellationToken token)
        {
            _cts = new CancellationTokenSource();

            _baseAnimSpeed = _config.BaseMoveSpeed / AnimationTargetMoveSpeed;

            IsActive = new BoolReactiveProperty();

            Position = new ReactiveProperty<Vector3>();
            Rotation = new ReactiveProperty<float>();
            
            AnimMultiplier = new ReactiveProperty<float>(_baseAnimSpeed);
            SfxAcceleration = new ReactiveProperty<float>();

            IsMoving = new BoolReactiveProperty();
            IsFall = new BoolReactiveProperty();

            SelectedSkin = new ReactiveProperty<string>(_config.DefaultSkinId);

            _updateService.Subscribe(this);

            return UniTask.CompletedTask;
        }

        #region SaveLoad

        SaveableData IProgressable.GetProgressData()
        {
            return new (SaveKey, new object[] {SelectedSkin.Value});
        }

        void IProgressable.RestoreProgress(SaveableData loadData)
        {
            if (loadData?.Data == null || loadData.Data.Length < 1)
            {
                Debug.LogError($"Can't restore Wallet data");
                return;
            }

            SelectedSkin.Value = Convert.ToString(loadData.Data[0]);
        }

        #endregion

        public void Update()
        {
            TryMove();
        }

        public void MoveTo(Vector2 position, float rotation = 0)
        {
            ResetState();
            Position.Value = position;
            Rotation.Value = rotation;
            IsActive.Value = true;
        }

        public void StartMove()
        {
            IsMoving.Value = true;
            _isVulnerable = true;
        }

        public void StopMove()
        {
            IsMoving.Value = false;
            _isVulnerable = false;
        }

        public void ResetState()
        {
            _isVulnerable = false;
            IsMoving.Value = false;
            IsFall.Value = false;
        }

        public void ChangeSpeed(int difficultyLevel)
        {
            var acceleration = difficultyLevel * _config.AccelerationByDiffLevel;
            _currentSpeed = _config.BaseMoveSpeed + acceleration;

            AnimMultiplier.Value = _baseAnimSpeed / _config.BaseMoveSpeed * _currentSpeed;
            SfxAcceleration.Value = difficultyLevel * SfxAccelerationStep;
        }

        public async UniTask CenteringAsync(Vector3 centre)
        {
            var origin = Position.Value;
            var t = 0f;

            while (t < 1f)
            {
                t += _config.CenteringSpeed * Time.deltaTime;

                if (IsPayerDirectionVecrtical())
                {
                    Position.Value = new Vector2(Mathf.Lerp(origin.x, centre.x, t), Position.Value.y);
                }
                else
                {
                    Position.Value = new Vector2(Position.Value.x, Mathf.Lerp(origin.y, centre.y, t));
                }

                await UniTask.NextFrame(_cts.Token);
            }
        }

        public async UniTask TurnAsync(float targetAngle)
        {
            var originAngle = Rotation.Value;
            var t = 0f;

            OnBeganTurning?.Invoke();

            while (t < 1f)
            {
                t += _config.TurnSpeed * Time.deltaTime;

                Rotation.Value = Mathf.LerpAngle(originAngle, originAngle + targetAngle, t);

                await UniTask.NextFrame(_cts.Token);
            }
        }

        public async UniTask JumpAsync()
        {
            _isVulnerable = false;
            OnBeganJumping?.Invoke();

            var t = Time.time;
            var totalJumpDuration = JumpDuration / AnimMultiplier.Value;

            while ((t + totalJumpDuration) >= Time.time)
            {
                await UniTask.NextFrame(_cts.Token);
            }

            _isVulnerable = true;
        }

        private void TryMove()
        {
            if (!IsMoving.Value) return;

            var rotatedDirection = Quaternion.Euler(0, 0, Rotation.Value) * MoveDirection;

            Position.Value += (rotatedDirection * (_currentSpeed * Time.deltaTime));
            _roadCheckerPosition = Position.Value + Quaternion.Euler(0, 0, Rotation.Value) * _config.RoadCheckerOffset;

            if (!CheckRoad())
            {
                _isVulnerable = false;
                IsMoving.Value = false;
                IsFall.Value = true;
            }
        }

        private bool CheckRoad()
        {
            return Physics2D.OverlapCircle(_roadCheckerPosition, 0.1f, 1 << 7) || _isVulnerable != true;
        }

        private bool IsPayerDirectionVecrtical()
        {
            return ((Mathf.Abs(Rotation.Value) < 5f) || (Mathf.Abs(Rotation.Value) > 355f) ||
                    (Mathf.Abs(Rotation.Value) > 175f && Mathf.Abs(Rotation.Value) < 185f));
        }

        public void Decompose()
        {
            _cts?.Cancel();
            _cts?.Dispose();

            _updateService.Unsubscribe(this);
        }
    }
}