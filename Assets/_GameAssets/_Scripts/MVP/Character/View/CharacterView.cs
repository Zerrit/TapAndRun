using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace TapAndRun.MVP.Character.View
{
    public class CharacterView : MonoBehaviour
    {
        public event Action OnFalling;

        [field:SerializeField] public Transform CharacterTransform { get; private set; }
        [field:SerializeField] public Transform SkinHandler { get; private set; }

        [SerializeField] private Transform _roadChecker;
        [SerializeField] private Animator _characterAnimator;

        private bool _isInteractive;
        private bool _isMoving;
        
        private float _currentSpeed;
        private float _animMultiplier;

        private float _baseMoveSpeed = 2f; // Вынести в конфиг
        private float _turnSpeed = 12f; // Вынести в конфиг
        private float _centeringSpeed = 4f; // Вынести в конфиг
        
        private const float BaseAnimSpeed = 1f;
        private const float JumpDuration = 1f;
        private static readonly Vector2 MoveDirection = Vector2.up;
        
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Run = Animator.StringToHash("Run");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Fall = Animator.StringToHash("Fall");
        private static readonly int JumpSpeed = Animator.StringToHash("Speed");

        public void Update()
        {
            TryMove();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Start"))
            {
                //isCanTapping = true;
            }
        }

        public void ChangeSpeed(int difficultyLevel)
        {
            _currentSpeed = _baseMoveSpeed + difficultyLevel;

            _animMultiplier = BaseAnimSpeed + difficultyLevel / _baseMoveSpeed;
            _characterAnimator.SetFloat(JumpSpeed, _animMultiplier);
            
            Debug.Log(_currentSpeed);
            Debug.Log(_animMultiplier);
        }
        
        public void StartMove()
        {
            _isMoving = true;
            _isInteractive = true;
            ActivateAnimation(Run);
        }

        public void StopMove()
        {
            _isInteractive = false;
            _isMoving = false;
            ActivateAnimation(Idle);
        }
        
        public void MoveTo(Vector2 position)
        {
            CharacterTransform.position = position;
            CharacterTransform.rotation = Quaternion.identity;
        }
        
        public async UniTaskVoid CenteringAsync(Vector3 centre)
        {
            var origin = CharacterTransform.position;
            var t = 0f;

            while (t < 1f)
            {
                t += _centeringSpeed * Time.deltaTime;

                if (IsPayerDirectionVecrtical())
                {
                    CharacterTransform.position = new Vector2(Mathf.Lerp(origin.x, centre.x, t), CharacterTransform.position.y);
                }
                else
                {
                    CharacterTransform.position = new Vector2(CharacterTransform.position.x, Mathf.Lerp(origin.y, centre.y, t));
                }

                await UniTask.NextFrame(this.GetCancellationTokenOnDestroy());
            }

            await UniTask.CompletedTask;
        }

        public async UniTaskVoid TurnAsync(float targetAngle)
        {
            var originAngle = CharacterTransform.eulerAngles.z;
            var t = 0f;

            while (t < 1f)
            {
                t += _turnSpeed * Time.deltaTime;

                var angle = Mathf.LerpAngle(originAngle, originAngle + targetAngle, t);
                CharacterTransform.eulerAngles = new Vector3(0, 0, angle);

                await UniTask.NextFrame(this.GetCancellationTokenOnDestroy());
            }

            await UniTask.CompletedTask;
        }

        public async UniTaskVoid JumpAsync()
        {
            _isInteractive = false;
            ActivateAnimation(Jump);

            var t = Time.time;
            var totalJumpDuration = JumpDuration / _animMultiplier;
            Debug.Log(totalJumpDuration);

            while ((t + (JumpDuration / _animMultiplier)) >= Time.time) //TODO добавить логику согласно изменению сложности
            {
                await UniTask.NextFrame(this.GetCancellationTokenOnDestroy());
            }

            _isInteractive = true;

            await UniTask.CompletedTask;
        }

        private bool IsPayerDirectionVecrtical()
        {
            var rotation = CharacterTransform.eulerAngles.z;

            return ((Mathf.Abs(rotation) < 5f) || (Mathf.Abs(rotation) > 355f) ||
                    (Mathf.Abs(rotation) > 175f && Mathf.Abs(rotation) < 185f));
        }

        private void TryMove()
        {
            if (!_isMoving) return;

            CharacterTransform.Translate(MoveDirection * (_currentSpeed * Time.deltaTime));

            if (!CheckRoad())
            {
                _isInteractive = false;
                _isMoving = false;
                _characterAnimator.SetTrigger(Fall);

                OnFalling?.Invoke();
            }
        }

        private bool CheckRoad()
        {
            if (Physics2D.OverlapCircle(_roadChecker.position, 0.1f, 1 << 7) || _isInteractive != true)
            {
                return true;
            }

            return false;
        }

        private void ActivateAnimation(int animHash)
        {
            _characterAnimator.SetTrigger(animHash);
        }
    }
}
