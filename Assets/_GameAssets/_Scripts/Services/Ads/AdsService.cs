using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Interfaces;
using UnityEngine;

namespace TapAndRun.Services.Ads
{
    public class AdsService : MonoBehaviour ,IAdsService, IDecomposable
    {
#if UNITY_ANDROID
        private string _appKey = "1f56a1b65";
#elif UNITY_IPHONE
        private string _appKey = "";
#else
        private string _appKey = "unexpected_platform";
#endif

        public UniTask InitializeAsync(CancellationToken token)
        {
            IronSource.Agent.validateIntegration();
            IronSource.Agent.init(_appKey);

            IronSourceEvents.onSdkInitializationCompletedEvent += ConfirmSdkInitilization;

            //Add AdInfo Banner Events
            IronSourceBannerEvents.onAdLoadedEvent += BannerOnAdLoadedEvent;
            IronSourceBannerEvents.onAdLoadFailedEvent += BannerOnAdLoadFailedEvent;
            IronSourceBannerEvents.onAdClickedEvent += BannerOnAdClickedEvent;
            IronSourceBannerEvents.onAdScreenPresentedEvent += BannerOnAdScreenPresentedEvent;
            IronSourceBannerEvents.onAdScreenDismissedEvent += BannerOnAdScreenDismissedEvent;
            IronSourceBannerEvents.onAdLeftApplicationEvent += BannerOnAdLeftApplicationEvent;

            //Add AdInfo Interstitial Events
            IronSourceInterstitialEvents.onAdReadyEvent += InterstitialOnAdReadyEvent;
            IronSourceInterstitialEvents.onAdLoadFailedEvent += InterstitialOnAdLoadFailed;
            IronSourceInterstitialEvents.onAdOpenedEvent += InterstitialOnAdOpenedEvent;
            IronSourceInterstitialEvents.onAdClickedEvent += InterstitialOnAdClickedEvent;
            IronSourceInterstitialEvents.onAdShowSucceededEvent += InterstitialOnAdShowSucceededEvent;
            IronSourceInterstitialEvents.onAdShowFailedEvent += InterstitialOnAdShowFailedEvent;
            IronSourceInterstitialEvents.onAdClosedEvent += InterstitialOnAdClosedEvent;

            //Add AdInfo Rewarded Video Events
            IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoOnAdOpenedEvent;
            IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;
            IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
            IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
            IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
            IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;

            return UniTask.CompletedTask;
        }

        public async UniTask ShowInterstitialAsync(CancellationToken token)
        {
            LoadInterstitial();

            await UniTask.WaitUntil(IsInterstitialReady, cancellationToken: token);

            ShowInterstitial();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            IronSource.Agent.onApplicationPause(pauseStatus);
        }

        private void ConfirmSdkInitilization()
        {
            Debug.Log("SDK is initialized!");
        }

        #region Banner

        public void LoadBanner()
        {
            IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
        }

        public void RemoveBanner()
        {
            IronSource.Agent.destroyBanner();
        }
        
        /************* Banner AdInfo Delegates *************/
        //Invoked once the banner has loaded
        void BannerOnAdLoadedEvent(IronSourceAdInfo adInfo) 
        {
        }
        //Invoked when the banner loading process has failed.
        void BannerOnAdLoadFailedEvent(IronSourceError ironSourceError) 
        {
        }
        // Invoked when end user clicks on the banner ad
        void BannerOnAdClickedEvent(IronSourceAdInfo adInfo) 
        {
        }
        //Notifies the presentation of a full screen content following user click
        void BannerOnAdScreenPresentedEvent(IronSourceAdInfo adInfo) 
        {
        }
        //Notifies the presented screen has been dismissed
        void BannerOnAdScreenDismissedEvent(IronSourceAdInfo adInfo) 
        {
        }
        //Invoked when the user leaves the app
        void BannerOnAdLeftApplicationEvent(IronSourceAdInfo adInfo) 
        {
        }

        #endregion

        #region Interstitial

        public void LoadInterstitial()
        {
            IronSource.Agent.loadInterstitial();
        }

        public bool IsInterstitialReady()
        {
            return IronSource.Agent.isInterstitialReady();
        }
        
        public void ShowInterstitial()
        {
            IronSource.Agent.showInterstitial();
        }

        /************* Interstitial AdInfo Delegates *************/
        // Invoked when the interstitial ad was loaded succesfully.
        void InterstitialOnAdReadyEvent(IronSourceAdInfo adInfo) {
        }
        // Invoked when the initialization process has failed.
        void InterstitialOnAdLoadFailed(IronSourceError ironSourceError) {
        }
        // Invoked when the Interstitial Ad Unit has opened. This is the impression indication. 
        void InterstitialOnAdOpenedEvent(IronSourceAdInfo adInfo) {
        }
        // Invoked when end user clicked on the interstitial ad
        void InterstitialOnAdClickedEvent(IronSourceAdInfo adInfo) {
        }
        // Invoked when the ad failed to show.
        void InterstitialOnAdShowFailedEvent(IronSourceError ironSourceError, IronSourceAdInfo adInfo) {
        }
        // Invoked when the interstitial ad closed and the user went back to the application screen.
        void InterstitialOnAdClosedEvent(IronSourceAdInfo adInfo) {
        }
        // Invoked before the interstitial ad was opened, and before the InterstitialOnAdOpenedEvent is reported.
        // This callback is not supported by all networks, and we recommend using it only if  
        // it's supported by all networks you included in your build. 
        void InterstitialOnAdShowSucceededEvent(IronSourceAdInfo adInfo) {
        }
        
        #endregion
        
        #region Rewardable

        public void LoadRewardable()
        {
            IronSource.Agent.loadRewardedVideo();
        }

        public void ShowRewardable()
        {
            if (IronSource.Agent.isRewardedVideoAvailable())
            {
                IronSource.Agent.showRewardedVideo();
            }
            else
            {
                Debug.Log("Rewardable is not ready");
            }
        }

        /************* RewardedVideo AdInfo Delegates *************/
        // Indicates that there’s an available ad.
        // The adInfo object includes information about the ad that was loaded successfully
        // This replaces the RewardedVideoAvailabilityChangedEvent(true) event
        void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo) {
        }
        // Indicates that no ads are available to be displayed
        // This replaces the RewardedVideoAvailabilityChangedEvent(false) event
        void RewardedVideoOnAdUnavailable() {
        }
        // The Rewarded Video ad view has opened. Your activity will loose focus.
        void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo){
        }
        // The Rewarded Video ad view is about to be closed. Your activity will regain its focus.
        void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo){
        }
        // The user completed to watch the video, and should be rewarded.
        // The placement parameter will include the reward data.
        // When using server-to-server callbacks, you may ignore this event and wait for the ironSource server callback.
        void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
        {
            Debug.Log("Получена награда за просмотр рекламы");
        }
        // The rewarded video ad was failed to show.
        void RewardedVideoOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo){
        }
        // Invoked when the video ad was clicked.
        // This callback is not supported by all networks, and we recommend using it only if
        // it’s supported by all networks you included in your build.
        void RewardedVideoOnAdClickedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo){
        }

        #endregion

        public void Decompose()
        {
            IronSourceEvents.onSdkInitializationCompletedEvent -= ConfirmSdkInitilization;
            
            //Unsub Banner Events
            IronSourceBannerEvents.onAdLoadedEvent -= BannerOnAdLoadedEvent;
            IronSourceBannerEvents.onAdLoadFailedEvent -= BannerOnAdLoadFailedEvent;
            IronSourceBannerEvents.onAdClickedEvent -= BannerOnAdClickedEvent;
            IronSourceBannerEvents.onAdScreenPresentedEvent -= BannerOnAdScreenPresentedEvent;
            IronSourceBannerEvents.onAdScreenDismissedEvent -= BannerOnAdScreenDismissedEvent;
            IronSourceBannerEvents.onAdLeftApplicationEvent -= BannerOnAdLeftApplicationEvent;
            
            //Unsub Interstitial Events
            IronSourceInterstitialEvents.onAdReadyEvent -= InterstitialOnAdReadyEvent;
            IronSourceInterstitialEvents.onAdLoadFailedEvent -= InterstitialOnAdLoadFailed;
            IronSourceInterstitialEvents.onAdOpenedEvent -= InterstitialOnAdOpenedEvent;
            IronSourceInterstitialEvents.onAdClickedEvent -= InterstitialOnAdClickedEvent;
            IronSourceInterstitialEvents.onAdShowSucceededEvent -= InterstitialOnAdShowSucceededEvent;
            IronSourceInterstitialEvents.onAdShowFailedEvent -= InterstitialOnAdShowFailedEvent;
            IronSourceInterstitialEvents.onAdClosedEvent -= InterstitialOnAdClosedEvent;
            
            //Unsub Rewarded Video Events
            IronSourceRewardedVideoEvents.onAdOpenedEvent -= RewardedVideoOnAdOpenedEvent;
            IronSourceRewardedVideoEvents.onAdClosedEvent -= RewardedVideoOnAdClosedEvent;
            IronSourceRewardedVideoEvents.onAdAvailableEvent -= RewardedVideoOnAdAvailable;
            IronSourceRewardedVideoEvents.onAdUnavailableEvent -= RewardedVideoOnAdUnavailable;
            IronSourceRewardedVideoEvents.onAdShowFailedEvent -= RewardedVideoOnAdShowFailedEvent;
            IronSourceRewardedVideoEvents.onAdRewardedEvent -= RewardedVideoOnAdRewardedEvent;
            IronSourceRewardedVideoEvents.onAdClickedEvent -= RewardedVideoOnAdClickedEvent;
        }
    }
}