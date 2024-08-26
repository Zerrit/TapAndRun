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

        void MoveTo(Vector2 position, float rotation = 0);
        void StartMove();
        void ResetState();

        void ChangeSpeed(int difficultyLevel);

        UniTask CenteringAsync(Vector3 centre);
        UniTask TurnAsync(float targetAngle);
        UniTask JumpAsync();
    }
}