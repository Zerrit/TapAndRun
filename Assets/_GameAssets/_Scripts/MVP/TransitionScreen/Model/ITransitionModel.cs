using TapAndRun.Interfaces;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.TransitionScreen.Model
{
    public interface ITransitionModel : IInitializableAsync
    {
        TriggerReactiveProperty StartTrigger { get; }
        bool IsScreenHidden { get; }
        bool CanFinish { set; }
    }
}