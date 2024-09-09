using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Interfaces;

namespace TapAndRun.Services.Ads
{
    public interface IAdsService : IInitializableAsync
    {
        void LoadInterstitialAd();
        bool IsInterstitialReady();
        void ShowInterstitial();
    }
}