using Cysharp.Threading.Tasks;
using TapAndRun.Interfaces;
using UnityEngine;

namespace TapAndRun.MVP.CharacterCamera.Model
{
    public interface ICameraModel : IInitializableAsync
    {
        int Difficulty { get; }

        void SetSpecialView(Vector3 position, float rotation, float height);
        void SetRotation(float rotation = 0);
        void ChangeDifficulty(int newDifficulty);

        UniTaskVoid TurnAsync(int direction);
        UniTaskVoid FlyUpAsync();
    }
}