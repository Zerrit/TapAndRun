using System;
using UnityEngine;

namespace TapAndRun.MVP.CharacterCamera.Model
{
    [Serializable]
    public class CameraModeData
    {
        [field:SerializeField] public CameraMode Mode { get; private set; }
        [field:SerializeField] public int Angle { get; private set; }
    }
}