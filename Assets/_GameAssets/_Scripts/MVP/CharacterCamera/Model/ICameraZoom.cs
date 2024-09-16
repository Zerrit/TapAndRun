using Cysharp.Threading.Tasks;
using TapAndRun.Tools.Reactivity;
using UnityEngine;

namespace TapAndRun.MVP.CharacterCamera.Model
{
    public interface ICameraZoom
    {
        ReactiveProperty<Vector3> Position { get; }
        ReactiveProperty<float> Rotation { get; }

        UniTaskVoid SetLoseZoomAsync();
        UniTaskVoid SetFreeViewZoomAsync();
        UniTaskVoid SetShopZoomAsync();
    }
}