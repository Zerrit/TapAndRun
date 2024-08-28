using System;
using TapAndRun.Tools.Reactivity;
using UnityEngine;

namespace TapAndRun.MVP.Character.Model
{
    public interface ISelfCharacterModel
    {
        event Action OnBeganTurning;
        event Action OnBeganJumping;
        event Action OnFinishedJumping;

        ReactiveProperty<Vector3> Position { get; }
        ReactiveProperty<float> Rotation { get; }
        ReactiveProperty<bool> IsMoving { get; }
        BoolReactiveProperty IsFall { get; }
        ReactiveProperty<float> AnimMultiplier { get; }
    }
}