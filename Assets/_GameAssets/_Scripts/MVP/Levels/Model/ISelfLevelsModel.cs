using System;

namespace TapAndRun.MVP.Levels.Model
{
    public interface ISelfLevelsModel
    { 
        event Action OnLevelReseted;
        event Action OnLevelChanged;
        event Action OnLevelStarted;

        int CurrentLevelId { get; }
        int LevelCount { get; set; }

        int CurrentDifficulty { get; }

        void LoseLevel();
        void CompleteLevel();

        bool CheckLevelExist(int levelId);
    }
}