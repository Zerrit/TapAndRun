using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Lose.Model
{
    public class LoseModel : ISelfLoseModel, ILoseModel
    {
        public TriggerReactiveProperty HomeTrigger { get; private set; }
        public TriggerReactiveProperty RestartTrigger { get; private set; }

        public ReactiveProperty<bool> IsDisplaying { get; private set; }

        public UniTask InitializeAsync(CancellationToken token)
        {
            IsDisplaying = new ReactiveProperty<bool>();
            HomeTrigger = new TriggerReactiveProperty();
            RestartTrigger = new TriggerReactiveProperty();

            return UniTask.CompletedTask;
        }
    }
}