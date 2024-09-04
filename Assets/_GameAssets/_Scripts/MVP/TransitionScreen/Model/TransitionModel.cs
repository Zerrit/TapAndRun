using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.TransitionScreen.Model
{
    public class TransitionModel : ITransitionModel, ISelfTransitionModel
    {
        public TriggerReactiveProperty StartTrigger { get; private set; }

        public bool CanFinish { get; set; }
        public bool IsScreenHidden { get; set; }

        public UniTask InitializeAsync(CancellationToken token)
        {
            StartTrigger = new TriggerReactiveProperty();
            
            return UniTask.CompletedTask;
        }
    }
}