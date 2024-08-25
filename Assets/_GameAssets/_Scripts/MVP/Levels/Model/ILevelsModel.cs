using System;
using TapAndRun.Interfaces;

namespace TapAndRun.MVP.Levels.Model
{
    public interface ILevelsModel : IInitializableAsync
    {
        event Action OnLevelFailed;

        void PrepeareCurrentLevel();
        void PrepareLevel(int levelId);
        void StartGameplay();
    }
}