using TapAndRun.Interfaces;

namespace TapAndRun.Services.Ads
{
    public interface IAdsService : IInitializableAsync
    {
        public void ReduceInterstitialAdCounter(int count = 1);
    }
}