using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Configs;
using TapAndRun.Interfaces;
using TapAndRun.Services.Update;
using TapAndRun.Tools.Reactivity;
using UnityEngine;

namespace TapAndRun.MVP.Character.Model
{
    public class CharacterModel : ISelfCharacterModel, ICharacterModel, IUpdatable, IDisposable
    {
        public event Action OnBeganTurning;
        public event Action OnBeganJumping;
        public event Action OnFinishedJumping;

        public ReactiveProperty<Vector3> Position { get; private set; }
        public ReactiveProperty<float> Rotation { get; private set; }
        public ReactiveProperty<bool> IsMoving { get; private set; }
        public BoolReactiveProperty IsFall { get; private set; }
        public ReactiveProperty<float> AnimMultiplier { get; private set; }

        private Vector3 _roadCheckerPosition;
        private float _currentSpeed;
        private bool _isVulnerable;

        private CancellationTokenSource _cts;

        private const float BaseAnimSpeed = 1f;
        private const float JumpDuration = 1f;

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
            Position = new ReactiveProperty<Vector3>();
            Rotation = new ReactiveProperty<float>();
            IsMoving = new ReactiveProperty<bool>();
            IsFall = new BoolReactiveProperty();
            AnimMultiplier = new ReactiveProperty<float>(BaseAnimSpeed);

            _updateService.Subscribe(this);
            
            return UniTask.CompletedTask;
        }

        public void Update()
        {
            TryMove();
        }

        public void MoveTo(Vector2 position, float rotation = 0)
        {
            ResetState();
            Position.Value = position;
            Rotation.Value = rotation;
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
            _currentSpeed = _config.BaseMoveSpeed + difficultyLevel;
            AnimMultiplier.Value = BaseAnimSpeed + (difficultyLevel / _config.BaseMoveSpeed);
            //CharacterSfx.ChangeSpeed(difficultyLevel);
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

            while ((t + totalJumpDuration) >= Time.time) //TODO добавить логику согласно изменению сложности
            {
                await UniTask.NextFrame(_cts.Token);
            }

            OnFinishedJumping?.Invoke();
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
            if (Physics2D.OverlapCircle(_roadCheckerPosition, 0.1f, 1 << 7) || _isVulnerable != true)
            {
                return true;
            }

            return false;
        }

        private bool IsPayerDirectionVecrtical()
        {
            return ((Mathf.Abs(Rotation.Value) < 5f) || (Mathf.Abs(Rotation.Value) > 355f) ||
                    (Mathf.Abs(Rotation.Value) > 175f && Mathf.Abs(Rotation.Value) < 185f));
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();

            _updateService.Unsubscribe(this);
        }
    }
}