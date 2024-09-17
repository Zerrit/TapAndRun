using System;
using UnityEngine;

namespace TapAndRun.Configs
{
    [CreateAssetMenu(fileName = "CameraConfig", menuName = "Camera/Camera Config")]
    public class CameraConfig : ScriptableObject
    {
        [field:SerializeField] public Vector3 Offset { get; private set; }
        [field:SerializeField] public float TurnSpeed { get; private set; }
        [field:SerializeField] public float ZoomSpeed { get; private set; }
        [field:SerializeField] public float Height { get; private set; }
        [field:SerializeField] public float HeightStep { get; private set; }

        [field:SerializeField] public float LoseHeight { get; private set; }
        [field:SerializeField] public float FreeViewHeight { get; private set; }
        [field:SerializeField] public float ShopHeight { get; private set; }

        [field:SerializeField] public CameraModeData[] CameraModes { get; private set; }
    }

    [Serializable]
    public class CameraModeData
    {
        [field:SerializeField] public CameraMode Mode { get; private set; }
        [field:SerializeField] public int Angle { get; private set; }
    }

    public enum CameraMode
    {
        Static = 0,
        PositiveLowAngle = 1,
        NegativeLowAngle = 2,
        PositiveAngle = 3,
        NegativeAngle = 4,
        Random = 5
    }
}