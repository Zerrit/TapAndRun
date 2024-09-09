using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Services.Ads;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Lose.Model
{
    public class LoseModel : ISelfLoseModel, ILoseModel
    {
        private readonly IAdsService _adsService;
        public TriggerReactiveProperty HomeTrigger { get; private set; }
        public TriggerReactiveProperty RestartTrigger { get; private set; }

        public ReactiveProperty<bool> IsDisplaying { get; private set; }

        public int _loseBeforeAds;
        
        private int _adsInterval = 3;
        
        public LoseModel(/*IAdsService adsService*/)
        {
            //_adsService = adsService;
        }
        
        public UniTask InitializeAsync(CancellationToken token)
        {
            IsDisplaying = new ReactiveProperty<bool>();
            HomeTrigger = new TriggerReactiveProperty();
            RestartTrigger = new TriggerReactiveProperty();

            return UniTask.CompletedTask;
        }

        public void ShowAds()
        {
            //_adsService.
        }
    }
}