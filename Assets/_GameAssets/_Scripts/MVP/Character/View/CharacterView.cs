using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TapAndRun.MVP.Character.View
{
    public class CharacterView : MonoBehaviour
    {
        public event Action OnFalling;

        public bool _isInteractive;
        
        [field:SerializeField] public Transform CharacterTransform { get; private set; }
        [field:SerializeField] public Transform SkinHandler { get; private set; }

        [SerializeField] private Transform _roadChecker;
        [SerializeField] private Animator _characterAnimator;

        private bool _isMoving;
        private float _currentSpeed;

        private float _turnSpeed = 12f; // Вынести в конфиг
        private float _centeringSpeed = 4f; // Вынести в конфиг
        
        private static readonly Vector2 MoveDirection = Vector2.up;
        
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Run = Animator.StringToHash("Run");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Fall = Animator.StringToHash("Fall");

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

        public void StartMove(float speed = 4)
        {
            _currentSpeed = speed;

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

            while ((t+0.5f) >= Time.time) //TODO добавить логику согласно изменению сложности
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
