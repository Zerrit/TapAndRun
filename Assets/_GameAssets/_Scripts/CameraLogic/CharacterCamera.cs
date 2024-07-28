using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.MVP.Levels.Model;
using UnityEngine;

namespace TapAndRun.CameraLogic
{
    public class CharacterCamera : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _smoothSpeed;

        [SerializeField] private Camera _camera;

        private Transform _character;

        public void SetCharacter(Transform character)
        {
            _character = character;
        }

        private void LateUpdate()
        {
            FollowCharacter();
        }

        private void FollowCharacter()
        {
            Vector3 targetPosition = _character.position + _offset;
            
            transform.position = targetPosition;
            /*Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, _smoothSpeed);
            transform.position = smoothedPosition;*/
        }

        private void TapRotation(InteractType commandType)
        {
            /*switch (cameraDificulty)
        {
            case 1:
                if (commandType == CommandType.TurnLeft) StartCoroutine(TurnCamera(camTransform.eulerAngles.z, 90f)); 
                if (commandType == CommandType.TurnRight) StartCoroutine(TurnCamera(camTransform.eulerAngles.z, -90f));   
                break;

            case 2:
                if (commandType == CommandType.TurnLeft) StartCoroutine(TurnCamera(camTransform.eulerAngles.z, 40f));
                if (commandType == CommandType.TurnRight) StartCoroutine(TurnCamera(camTransform.eulerAngles.z, -40f));
                break;

            case 3:
                if (commandType == CommandType.TurnLeft) StartCoroutine(TurnCamera(camTransform.eulerAngles.z, Random.Range(-100, 100)));
                if (commandType == CommandType.TurnRight) StartCoroutine(TurnCamera(camTransform.eulerAngles.z, Random.Range(-100, 100)));
                if (commandType == CommandType.Jump) StartCoroutine(TurnCamera(camTransform.eulerAngles.z, Random.Range(-200, 200)));
                break;
        }*/
        }

        /*private void ChangeCameraDistance(int value)
        {
            StartCoroutine(ResizeCamera((_camera).orthographicSize, value * .5f));
        }

        public void ChangeCameraDificulty()
        {
            cameraDificulty = Mathf.Clamp(++cameraDificulty, 1, 3);
            ChangeCameraDistance(cameraDificulty);
        }
        public void ChangeCameraDificulty(int value)
        {
            cameraDificulty = value;
            ChangeCameraDistance(value);
        }*/


        public async UniTask TurnAsync(float angle, CancellationToken token)
        {
            var originRotation = _camera.transform.eulerAngles.z;
            float t = 0;

            while (t < 1)
            {
                t += 10 * Time.deltaTime;
                _camera.transform.eulerAngles = new Vector3(0, 0, Mathf.LerpAngle(originRotation, originRotation + angle, t));

                UniTask.Yield(token);
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
                _camera.orthographicSize = Mathf.Lerp(originDistance, 3.75f + target, t);

                UniTask.Yield(token);
            }

            await UniTask.CompletedTask;
        }

        private void SetDefault()
        {
            _camera.transform.rotation = _character.rotation;
            _camera.orthographicSize = 4; //TODO 
        }
    }
}
