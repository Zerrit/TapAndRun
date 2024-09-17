using System;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Levels.Model
{
    public interface ISelfLevelsModel
    {
        event Action OnLevelChanged;

        BoolReactiveProperty IsDisplaying { get; }
        BoolReactiveProperty IsTutorialDisplaying { get; }

        TriggerReactiveProperty StartupTrigger { get; }
        TriggerReactiveProperty ResetLevelTrigger { get; } 

        TriggerReactiveProperty OnTapTrigger { get; }
        TriggerReactiveProperty OnEnterToInteractPointTrigger { get; }

        int CurrentLevelId { get; }
        int LevelCount { get; set; }

        int CurrentDifficulty { get; set; }

        bool IsTutorialLevel { get; set; }
        bool IsTutorialComplete { get; set; }

        void AddCrystalByRun();
        void LoseLevel();
        void CompleteLevel();

        bool CheckLevelExist(int levelId);
    }
}