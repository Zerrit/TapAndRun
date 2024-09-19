using TapAndRun.Tools.Reactivity;
using UnityEngine;

namespace TapAndRun.MVP.CharacterCamera.Model
{
    public interface ISelfCameraModel
    {
        ReactiveProperty<Vector3> Position { get; }
        ReactiveProperty<float> Rotation { get; }
        ReactiveProperty<float> Height { get; }
    }
}