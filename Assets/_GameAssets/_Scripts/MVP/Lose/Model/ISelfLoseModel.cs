using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Lose.Model
{
    public interface ISelfLoseModel
    {
        ReactiveProperty<bool> IsDisplaying { get; }

        TriggerReactiveProperty HomeTrigger { get; }
        TriggerReactiveProperty RestartTrigger { get; }
    }
}