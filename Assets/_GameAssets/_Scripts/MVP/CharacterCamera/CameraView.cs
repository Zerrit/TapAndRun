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
            Debug.Log($"Изменение высоты камеры {height}");
            Camera.orthographicSize = height;
        }
    }
}
