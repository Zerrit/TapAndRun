using System;
using Cysharp.Threading.Tasks;
using TapAndRun.Interfaces;
using TapAndRun.Tools.Reactivity;
using UnityEngine;

namespace TapAndRun.MVP.Character.Model
{
    public interface ICharacterModel : IInitializableAsync
    {
        BoolReactiveProperty IsActive { get; }
        ReactiveProperty<Vector3> Position { get; }
        ReactiveProperty<float> Rotation { get; }
        BoolReactiveProperty IsFall { get; }

        ReactiveProperty<string> SelectedSkin { get; }

        void MoveTo(Vector2 position, float rotation = 0);
        void StartMove();
        void StopMove();

        void ChangeSpeed(int difficultyLevel);

        UniTask CenteringAsync(Vector3 centre);
        UniTask TurnAsync(float targetAngle);
        UniTask JumpAsync();
    }
}