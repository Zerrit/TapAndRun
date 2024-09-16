using Cysharp.Threading.Tasks;
using TapAndRun.Interfaces;
using TapAndRun.Tools.Reactivity;
using UnityEngine;

namespace TapAndRun.MVP.CharacterCamera.Model
{
    public interface ICameraModel : IInitializableAsync
    {
        int Difficulty { get; }

        void SetRotation(float rotation = 0);
        void ChangeDifficulty(int newDifficulty);

        UniTaskVoid TurnAsync(int direction);
    }
}