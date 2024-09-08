using System;
using TapAndRun.Interfaces;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Levels.Model
{
    public interface ILevelsModel : IInitializableAsync
    {
        event Action OnLevelFailed;

        BoolReactiveProperty IsDisplaying { get; }

        TriggerReactiveProperty StartupTrigger { get; }
        TriggerReactiveProperty ResetLevelTrigger { get; } 

        int LastUnlockedLevelId { get; }
        int LevelCount { get; }

        void SelectLevel(int levelId);
    }
}