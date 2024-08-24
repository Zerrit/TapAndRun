using TapAndRun.Tools.Reactivity;
using UnityEngine;

namespace TapAndRun.MVP.CharacterCamera
{
    public interface ISelfCameraModel
    {
        SimpleReactiveProperty<Vector3> Position { get; }
        SimpleReactiveProperty<float> Rotation { get; }
        SimpleReactiveProperty<float> Height { get; }
    }
}