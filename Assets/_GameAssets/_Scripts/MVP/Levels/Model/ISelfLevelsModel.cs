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

        int CurrentLevelId { get; }
        int LevelCount { get; set; }
        int CurrentDifficulty { get; }

        bool IsTutorialComplete { get; set; }

        void AddCrystalByRun();
        void LoseLevel();
        void CompleteLevel();

        bool CheckLevelExist(int levelId);
    }
}