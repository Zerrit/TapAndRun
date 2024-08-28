using System;
using TapAndRun.Interfaces;

namespace TapAndRun.MVP.Levels.Model
{
    public interface ILevelsModel : IInitializableAsync
    {
        event Action OnLevelFailed;
        
        int LastUnlockedLevelId { get; }
        int LevelCount { get; set; }

        void PrepeareCurrentLevel();
        void PrepareLevel(int levelId);
        void StartGameplay();
    }
}