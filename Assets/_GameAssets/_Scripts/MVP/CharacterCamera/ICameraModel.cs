using Cysharp.Threading.Tasks;
using TapAndRun.Interfaces;

namespace TapAndRun.MVP.CharacterCamera
{
    public interface ICameraModel : IInitializableAsync
    {
        int Difficulty { get; }

        void SetRotation(float rotation = 0);
        void ChangeDifficulty(int newDifficulty);
        UniTaskVoid TurnAsync(int direction);
        UniTaskVoid FlyUpAsync();
    }
}