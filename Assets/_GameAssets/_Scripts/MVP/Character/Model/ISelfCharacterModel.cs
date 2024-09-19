using System;
using TapAndRun.Tools.Reactivity;
using UnityEngine;

namespace TapAndRun.MVP.Character.Model
{
    public interface ISelfCharacterModel
    {
        event Action OnBeganTurning;
        event Action OnBeganJumping;

        BoolReactiveProperty IsActive { get; }

        ReactiveProperty<Vector3> Position { get; }
        ReactiveProperty<float> Rotation { get; }

        ReactiveProperty<float> AnimMultiplier { get; }
        ReactiveProperty<float> SfxAcceleration { get; }

        BoolReactiveProperty IsMoving { get; }
        BoolReactiveProperty IsFall { get; }

        ReactiveProperty<string> SelectedSkin { get; }
    }
}