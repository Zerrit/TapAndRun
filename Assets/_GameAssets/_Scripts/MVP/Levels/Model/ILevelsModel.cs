using System;
using TapAndRun.Interfaces;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Levels.Model
{
    public interface ILevelsModel : IInitializableAsync
    {
        event Action OnLevelFailed;
        
        TriggerReactiveProperty StartupTrigger { get; }
        TriggerReactiveProperty RemoveTrigger { get; }
        
        int LastUnlockedLevelId { get; }
        int LevelCount { get; set; }

        void PrepeareCurrentLevel();
        void PrepareLevel(int levelId);
    }
}