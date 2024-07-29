using System;
using System.Collections.Generic;
using TapAndRun.Tools;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Levels.Model
{
    public interface ILevelsSelfModel
    {
        event Action OnLevelChanged;
        event Action OnLevelStarted;
        event Action OnLevelCompleted;

        int CurrentLevelId { get; }
        bool IsLevelBuild { set; }
        
        void SetCommands(List<InteractType> interactions);
        void CompleteLevel();
        void ApplyClick();
    }
}