using System;
using TapAndRun.MVP.CharacterCamera.Model;
using UnityEngine;

namespace TapAndRun.Configs
{
    [Serializable]
    public class CameraModeData
    {
        [field:SerializeField] public CameraMode Mode { get; private set; }
        [field:SerializeField] public int Angle { get; private set; }
    }
}