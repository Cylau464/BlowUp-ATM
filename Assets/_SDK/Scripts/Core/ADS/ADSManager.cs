using apps.exception;
using System;
using UnityEngine;

namespace apps
{
    public static class ADSManager
    {
        #region variables
        /// <summary>
        /// To check if ADSManager is intialized.
        /// </summary>
        public static bool isInited { get; private set; }

        private static bool s_AllowAutoInterstitial;
        private static float s_LastTimerAds = 0;
        private static float s_ShowInterstitialEvery = 45;
        private static string s_InterstitialPlacement = "";
        private static string s_RewardVideoPlacement = "";

        private static Action s_OnCompletedReward;
        private static IADS s_ADSMaker;

        /// <summary>
        /// To enable and disable the Interstitial ads. 
        /// </summary>
        public static bool enableInterstitial
        {
            get { return s_ADSMaker.useInterstitial; }
            set { s_ADSMaker.useInterstitial = value; }
        }

        /// <summary>
        /// To enable and disable the Banner ads. 
        /// </summary>
        public static bool enableBanner
        {
            get { return s_ADSMaker.useBanner; }
            set
            {
                if (s_ADSMaker.useBanner != value)
                {
                    if (s_ADSMaker.useBanner == false)
                        s_ADSMaker.HideBanner();

                    s_ADSMaker.useBanner = value;
                }
            }
        }

        /// <summary>
        /// To enable and disable the RewardedVideo ads. 
        /// </summary>
        public static bool enableRewardedVideo
        {
            get { return s_ADSMaker.useRewardedVideo; }
            set { s_ADSMaker.useRewardedVideo = value; }
        }
        #endregion

        #region Initialize
        /// <summary>
        /// Intialize the ADSManager.
        /// </summary>
        /// <param name="adsMaker"> The IADS that will execute ADS. </param>
        /// <param name="showInterstitialEvery"> We will show ADS every amount of time in case of auto interstitial.
        /// And we will block the ADS that will called and has less then this time amount. </param>
        public static void Initialize(IADS adsMaker, bool autoInterstitial = false, float showInterstitialEvery = 0)
        {
            if (isInited)
                throw new ReinitializedException();

            if (adsMaker == null)
                throw new ArgumentNullException();

            s_AllowAutoInterstitial = 0 < showInterstitialEvery && autoInterstitial;
            isInited = true;
            s_ADSMaker = adsMaker;
            s_LastTimerAds = Time.time;
            s_ShowInterstitialEvery = showInterstitialEvery;
        }
        #endregion

        #region Interstitial
        /// <summary>
        /// Check if we can show an Interstitial.
        /// </summary>
        /// <returns> true Mean we can show. </returns>
        public static bool AllowShowInterstitial()
        {
            return isInited && s_ADSMaker.IsInterstitialAvailable();
        }

        /// <summary>
        /// To show Interstitial with placement name if it's is ready to show.
        /// </summary>
        /// <param name="placementName"> The placement name what we will show to understand the current ad. </param>
        public static bool ShowInterstitial(string placementName)
        {
            if (placementName == null || placementName.CompareTo("") == 0)
                throw new ArgumentEmptryOrNullException();

            if (!AllowShowInterstitial())
                return false;

            s_InterstitialPlacement = placementName;
            s_ADSMaker.ShowInterstitial(placementName, InterstitialAdClosed);
            return true;
        }

        /// <summary>
        /// To auto show Interstitial with placement name if it's pass the amount of time. if time not pass we will block showing ad.
        /// </summary>
        /// <param name="placement"> The placement name what we will show to understand the current ad. </param>
        public static bool AutoShowInterstitial(string placementName)
        {
            if (placementName == null || placementName.CompareTo("") == 0)
                throw new ArgumentEmptryOrNullException();

            if (s_AllowAutoInterstitial == false)
                return false;

            if (s_LastTimerAds + s_ShowInterstitialEvery <= Time.time)
            {
                if (ShowInterstitial(placementName))
                {
                    s_LastTimerAds = Time.time;
                    return true;
                }
            }

            return false;
        }

        private static void InterstitialAdClosed()
        {
            s_InterstitialPlacement = "";
        }
        #endregion

        #region banner
        /// <summary>
        /// To check it we can use banner.
        /// </summary>
        public static bool AllowShowBanner()
        {
            return isInited && s_ADSMaker.useBanner;
        }

        /// <summary>
        /// To Display banner.
        /// </summary>
        public static bool DisplayBanner()
        {
            if (!AllowShowBanner())
                return false;

            s_ADSMaker.DisplayBanner();
            return true;
        }

        /// <summary>
        /// To hide banner.
        /// </summary>
        public static void HideBanner()
        {
            s_ADSMaker.HideBanner();
        }
        #endregion

        #region RewardsVideo
        /// <summary>
        /// To check it we can use RewardedVideo.
        /// </summary>
        public static bool AllowShowRewardedVideo()
        {
            return isInited && s_ADSMaker.useRewardedVideo;
        }

        /// <summary>
        /// To show Interstitial with placement name.
        /// </summary>
        /// <param name="placementName"> The placement name what we will show to understand the current ad. </param>
        /// <param name="onRewarded"> The event executed on use close the ad. </param>
        /// <returns> Return true if the ad is showed </returns>
        public static bool ShowRewardedVideo(string placementName, Action onRewarded)
        {
            if (placementName == null || placementName.CompareTo("") == 0)
                throw new ArgumentEmptryOrNullException();

            if (!s_ADSMaker.IsRewardedideoAvailable())
                return false;

            s_OnCompletedReward = onRewarded;
            s_RewardVideoPlacement = placementName;
            s_ADSMaker.ShowRewardedVideo(placementName, OnRewardedVideoAdCompleted);
            return true;
        }
        #endregion

        #region Callback handlers
        private static void OnRewardedVideoAdCompleted()
        {
            s_RewardVideoPlacement = "";
            s_OnCompletedReward?.Invoke();
        }
        #endregion
    }
}
