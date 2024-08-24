using System;
using Cysharp.Threading.Tasks;
using TapAndRun.Interfaces;
using TapAndRun.Tools.Reactivity;
using UnityEngine;

namespace TapAndRun.MVP.Character.Model
{
    public interface ICharacterModel : IInitializableAsync
    {
        public event Action OnFalled;

        SimpleReactiveProperty<Vector3> Position { get; }
        SimpleReactiveProperty<float> Rotation { get; }

        void MoveTo(Vector2 position, float rotation);
        void StartMove();
        void SetIdle();

        void ChangeSpeed(int difficultyLevel);

        UniTaskVoid CenteringAsync(Vector3 centre);
        UniTaskVoid TurnAsync(float targetAngle);
        UniTaskVoid JumpAsync();
    }
}