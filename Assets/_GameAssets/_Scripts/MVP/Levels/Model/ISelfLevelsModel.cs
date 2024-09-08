using System;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Levels.Model
{
    public interface ISelfLevelsModel
    {
        event Action OnLevelChanged;

        BoolReactiveProperty IsDisplaying { get; }

        TriggerReactiveProperty StartupTrigger { get; }
        TriggerReactiveProperty ResetLevelTrigger { get; } 
        
        int CurrentLevelId { get; set; }
        int LevelCount { get; set; }

        int CurrentDifficulty { get; }

        void LoseLevel();
        void CompleteLevel();

        bool CheckLevelExist(int levelId);
    }
}