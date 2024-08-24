using UnityEngine;

namespace TapAndRun.MVP.CharacterCamera
{
    public class CameraView : MonoBehaviour
    {
        [field:SerializeField] public Transform Transform { get; private set; }
        [field:SerializeField] public Camera Camera { get; private set; }


        public void UpdatePosition(Vector3 position)
        {
            Transform.position = position;
        }

        public void UpdateRotation(float rotation)
        {
            var rotationEuler = Quaternion.Euler(0f, 0f, rotation);
            Transform.rotation = rotationEuler;
        }
        
        public void UpdateHeight(float height)
        {
            Camera.orthographicSize = height;
        }

        public void Initialize(Transform character)
        {
            //_maxDifficultyLevel = _config.RotationDifficulties.Length;
            //_camera.orthographicSize = _config.Height;
            
            //Difficulty = 1;
        }



        /*public async UniTask FlyUpAsync(CancellationToken token)
        {
            await ChangeDistanceAsync(_config.LoseHeight, token);
        }

        public async UniTask TurnAsync(int direction)
        {
            var originRotation = _camera.transform.eulerAngles.z;
            var angle = _config.RotationDifficulties[Difficulty - 1] * direction;
            
            Debug.Log(angle);
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
        }*/
    }
}
