using System;
using TapAndRun.Interfaces;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Lose.Model
{
    public interface ILoseModel : IInitializableAsync
    {
        ReactiveProperty<bool> IsDisplaying { get; }

        TriggerReactiveProperty HomeTrigger { get; }
        TriggerReactiveProperty RestartTrigger { get; }
    }
}