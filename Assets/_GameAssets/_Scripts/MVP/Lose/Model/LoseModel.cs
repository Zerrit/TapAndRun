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

        public int _loseBeforeAds;

        private int _adsInterval = 2;

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

            _loseBeforeAds = _adsInterval;
            
            return UniTask.CompletedTask;
        }

        public void IncreaseLoseCount()
        {
            _loseBeforeAds--;

            if (_loseBeforeAds == 0)
            {
                ShowAds();

                _loseBeforeAds = _adsInterval;
            }
        }
        
        public void ShowAds()
        {
            _adsService.ShowInterstitial(); //TODO Доработать систему показа рекламы при череде поражений
        }
    }
}