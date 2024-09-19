using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Services.Ads;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Lose.Model
{
    public class LoseModel : ISelfLoseModel, ILoseModel
    {
        public ReactiveProperty<bool> IsDisplaying { get; private set; }

        public TriggerReactiveProperty HomeTrigger { get; private set; }
        public TriggerReactiveProperty RestartTrigger { get; private set; }

        private readonly IAdsService _adsService;

        public LoseModel(IAdsService adsService)
        {
            _adsService = adsService;
        }

        public UniTask InitializeAsync(CancellationToken token)
        {
            IsDisplaying = new ReactiveProperty<bool>();
            HomeTrigger = new TriggerReactiveProperty();
            RestartTrigger = new TriggerReactiveProperty();

            return UniTask.CompletedTask;
        }

        public void ProcessLose()
        {
            _adsService.ReduceInterstitialAdCounter();
        }
    }
}