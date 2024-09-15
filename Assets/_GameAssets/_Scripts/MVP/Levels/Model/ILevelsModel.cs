using System;
using TapAndRun.Interfaces;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Levels.Model
{
    public interface ILevelsModel : IInitializableAsync
    {
        event Action OnLevelFailed;
        event Action OnCrystalTaken;

        BoolReactiveProperty IsDisplaying { get; }
        BoolReactiveProperty IsTutorialDisplaying { get; }
        
        TriggerReactiveProperty StartupTrigger { get; }
        TriggerReactiveProperty ResetLevelTrigger { get; } 

        TriggerReactiveProperty OnTapTrigger { get; }
        TriggerReactiveProperty OnEnterToInteractPointTrigger { get; } 
        
        bool IsTutorialComplete { get; }

        int LastUnlockedLevelId { get; }
        int LevelCount { get; }

        void SelectLevel(int levelId);
    }
}