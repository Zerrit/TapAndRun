using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TapAndRun.MVP.Character.View
{
    public class CharacterView : MonoBehaviour
    {
        public event Action OnLevelStarted;
        public event Action OnLevelFinished;
        public event Action OnCrystalTaken;

        [field:SerializeField] public Transform CharacterTransform { get; private set; }
        [field:SerializeField] public Transform SkinHandler { get; private set; }
        [field:SerializeField] public Transform RoadChecker { get; private set; }
        [field:SerializeField] public Animator CharacterAnimator { get; private set; }

        private static readonly int Run = Animator.StringToHash("Run");
        private static readonly int Jump = Animator.StringToHash("Jump");

        private bool _isInteractive;
        
        private bool _isMoving;
        private Vector2 _currentDirection;
        private float _currentSpeed;
        
        private float _turnSpeed = 12f; // Вынести в конфиг
        private float _centeringSpeed = 7f; // Вынести в конфиг

        public void Update()
        {
            TryMove();
        }

        public void ActiveRunAnimation()
        {
            CharacterAnimator.SetTrigger(Run);
        }

        public void ActiveJumpAnimation()
        {
            CharacterAnimator.SetTrigger(Jump);
        }

        public void StartMove(Vector2 startDirection, float speed)
        {
            _currentDirection = startDirection;
            _currentSpeed = speed;

            ActiveRunAnimation();
            _isMoving = true;
        }
        
        public async UniTask CenteringAsync(Vector3 centre)
        {
            _isInteractive = false;

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

                UniTask.Yield();
            }

            await UniTask.CompletedTask;
        }
        
        public async UniTask TurnAsync(float targetAngle)
        {
            var t = 0f;

            while (t < 1f)
            {
                t += _turnSpeed * Time.deltaTime;
                var originAngle = CharacterTransform.eulerAngles.z;
                var angle = Mathf.LerpAngle(originAngle, originAngle + targetAngle, t);

                CharacterTransform.eulerAngles = new Vector3(0, 0, angle);

                UniTask.Yield();
            }

            await UniTask.CompletedTask;
        }

        public async UniTask JumpAsync()
        {
            //_model.IsActive = false;
            ActiveJumpAnimation();

            var t = Time.time;

            while ((t+3f) <= Time.time) //TODO добавить логику согласно изменению сложности
            {
                UniTask.Yield();
            }

            ActiveRunAnimation();
            //_model.IsActive = true;

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

            CharacterTransform.Translate(_currentDirection * (_currentSpeed * Time.deltaTime));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Start"))
            {
                OnLevelStarted?.Invoke();
            }

            if (collision.CompareTag("Crystal"))
            {
                OnCrystalTaken?.Invoke();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Start"))
            {
                //isCanTapping = true;
            }
        }
    }
}
