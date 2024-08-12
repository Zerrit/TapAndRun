using TapAndRun.CameraLogic;
using UnityEngine;

namespace TapAndRun.Configs
{
    [CreateAssetMenu(fileName = "CameraConfig", menuName = "Camera/Camera Config")]
    public class CameraConfig : ScriptableObject
    {
        [field:SerializeField] public Vector3 Offset { get; private set; }
        [field:SerializeField] public float TurnSpeed { get; private set; }
        [field:SerializeField] public float Height { get; private set; }
        [field:SerializeField] public float HeightStep { get; private set; }
        [field:SerializeField] public float LoseHeight { get; private set; }
        [field:SerializeField] public int[] RotationDifficulties { get; private set; }
    }
}