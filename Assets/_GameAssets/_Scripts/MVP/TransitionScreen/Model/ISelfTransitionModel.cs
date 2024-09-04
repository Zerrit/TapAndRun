using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.TransitionScreen.Model
{
    public interface ISelfTransitionModel
    {
        TriggerReactiveProperty StartTrigger { get; }
        bool IsScreenHidden { set; }
        bool CanFinish { get; }
    }
}