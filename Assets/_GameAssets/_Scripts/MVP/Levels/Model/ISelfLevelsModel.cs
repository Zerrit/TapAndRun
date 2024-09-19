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

        ReactiveProperty<int> CurrentDifficulty { get; }

        int CurrentLevelId { get; }
        int NextLevelId { get; }
        int LevelCount { set; }

        bool IsTutorialLevel { set; }
        bool IsTutorialComplete { get; }

        void AddCrystalByRun();
        void LoseLevel();
        void CompleteLevel();
    }
}