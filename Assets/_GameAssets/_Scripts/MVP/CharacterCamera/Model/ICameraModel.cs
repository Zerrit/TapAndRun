using Cysharp.Threading.Tasks;
using TapAndRun.Configs;
using TapAndRun.Interfaces;
using TapAndRun.Tools.Reactivity;
using UnityEngine;

namespace TapAndRun.MVP.CharacterCamera.Model
{
    public interface ICameraModel : IInitializableAsync
    {
        void SetRotation(float rotation = 0);
        void ChangeMode(int characterSpeedLevel, CameraMode mode);

        CameraMode CurrentMode { get; }

        UniTaskVoid TurnAsync(int direction);
    }
}