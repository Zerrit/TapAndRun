using System;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Levels.Model
{
    public interface ISelfLevelsModel
    { 
        event Action OnLevelReseted;
        event Action OnLevelChanged;

        TriggerReactiveProperty StartupTrigger { get; }
        TriggerReactiveProperty RemoveTrigger { get; }
        
        int CurrentLevelId { get; set; }
        int LevelCount { get; set; }

        int CurrentDifficulty { get; }

        void LoseLevel();
        void CompleteLevel();

        bool CheckLevelExist(int levelId);
    }
}