using System;
using TapAndRun.Interfaces;

namespace TapAndRun.MVP.Levels.Model
{
    public interface ILevelsModel : IInitializableAsync
    {
        event Action OnLevelFailed;

        void LoadLevel();
        void LoadLevel(int levelId);
        void StartGameplay();
    }
}