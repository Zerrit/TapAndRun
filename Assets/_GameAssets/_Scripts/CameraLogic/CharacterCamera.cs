using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Configs;
using UnityEngine;

namespace TapAndRun.CameraLogic
{
    public class CharacterCamera : MonoBehaviour
    {
        public int Difficulty { get; private set; }

        [SerializeField] private CameraConfig _config;
        [SerializeField] private Camera _camera;

        private int _maxDifficultyLevel;
        private Transform _character;

        private CancellationTokenSource _cts;

        public void Initialize(Transform character)
        {
            _character = character;
            _cts = new CancellationTokenSource();
            
            _maxDifficultyLevel = _config.RotationDifficulties.Length;
            _camera.orthographicSize = _config.Height;
            
            Difficulty = 1;
        }

        private void LateUpdate()
        {
            FollowCharacter();
        }

        private void FollowCharacter()
        {
            if (!_character)
            {
                return;
            }
            
            Vector3 targetPosition = _character.position + _config.Offset;
            transform.position = targetPosition;
        }

        public void ChangeRotation(Quaternion rotation = default)
        {
            _camera.transform.rotation = rotation;
        }
        
        public void ChangeDifficulty(int newDifficulty)
        {
            Difficulty = Mathf.Clamp(newDifficulty, 1, _maxDifficultyLevel);
            var targetHeight = Difficulty * _config.HeightStep;

            ChangeDistanceAsync(targetHeight, _cts.Token).Forget();
        }

        public async UniTask FlyUpAsync(CancellationToken token)
        {
            await ChangeDistanceAsync(_config.LoseHeight, token);
        }

        public async UniTask TurnAsync(int direction)
        {
            var originRotation = _camera.transform.eulerAngles.z;
            var angle = _config.RotationDifficulties[Difficulty - 1 * direction];
            float t = 0;

            while (t < 1)
            {
                t += _config.TurnSpeed * Time.deltaTime;
                _camera.transform.eulerAngles = new Vector3(0, 0, Mathf.LerpAngle(originRotation, originRotation + angle, t));

                await UniTask.NextFrame(this.GetCancellationTokenOnDestroy());
            }

            await UniTask.CompletedTask;
        }
        
        public async UniTask TurnAsync(float angle)
        {
            var originRotation = _camera.transform.eulerAngles.z;
            float t = 0;

            while (t < 1)
            {
                t += _config.TurnSpeed * Time.deltaTime;
                _camera.transform.eulerAngles = new Vector3(0, 0, Mathf.LerpAngle(originRotation, originRotation + angle, t));

                await UniTask.NextFrame(this.GetCancellationTokenOnDestroy());
            }

            await UniTask.CompletedTask;
        }

        public async UniTask ChangeDistanceAsync(float target, CancellationToken token)
        {
            var originDistance = _camera.orthographicSize;
            float t = 0;

            while (t < 1)
            {
                t += 3 * Time.deltaTime;
                _camera.orthographicSize = Mathf.Lerp(originDistance, _config.Height + target, t);

                await UniTask.NextFrame(token);
            }

            await UniTask.CompletedTask;
        }
    }
}
