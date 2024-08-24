using System;
using TapAndRun.Tools.Reactivity;
using UnityEngine;

namespace TapAndRun.MVP.Character.Model
{
    public interface ISelfCharacterModel
    {
        event Action OnBeganIdle;
        event Action OnBeganRunning;
        event Action OnBeganTurning;
        event Action OnBeganJumping;
        event Action OnFalled;

        SimpleReactiveProperty<Vector3> Position { get; }
        SimpleReactiveProperty<float> Rotation { get; }

        SimpleReactiveProperty<float> AnimMultiplier { get; }
    }
}