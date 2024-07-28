using System;
using System.Collections.Generic;
using TapAndRun.Tools;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Levels.Model
{
    public interface ILevelsSelfModel
    {
        event Action OnLevelChanged;
        event Action OnLevelCompleted;
        
        SimpleReactiveProperty<bool> IsScreenDisplaying { get; }

        int CurrentLevelId { get; }
        bool IsLevelBuild { set; }
        
        void SetCommands(List<InteractType> interactions);

        void ApplyClick();
    }
}