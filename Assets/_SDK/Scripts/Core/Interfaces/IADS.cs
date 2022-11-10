using System;

namespace apps
{
    public interface IADS
    {
        /// <summary>
        /// For enable and disable use banner.
        /// </summary>
        bool useBanner { get; set; }
        /// <summary>
        /// For enable and disable use interstitial.
        /// </summary>
        bool useInterstitial { get; set; }
        /// <summary>
        /// For enable and disable use RewardedVideo.
        /// </summary>
        bool useRewardedVideo { get; set; }

        /// <summary>
        /// To Destroy banner.
        /// </summary>
        void DestroyBanner();

        /// <summary>
        /// To Display banner.
        /// </summary>
        void DisplayBanner();
        /// <summary>
        /// To hide banner.
        /// </summary>
        void HideBanner();

        /// <summary>
        /// To start (re)load Interstitial.
        /// </summary>
        void LoadInterstitial();
        /// <summary>
        /// To start (re)load RewardedVideo.
        /// </summary>
        void LoadRewardedVideo();

        /// <summary>
        /// To check if the interstitial is available and ready to show.
        /// </summary>
        bool IsInterstitialAvailable();
        /// <summary>
        /// To check if the Rewardedideo is available and ready to show.
        /// </summary>
        bool IsRewardedideoAvailable();

        /// <summary>
        /// To show Interstitial with placement name.
        /// </summary>
        /// <param name="placementName"> The placement name what we will show to understand the current ad. </param>
        /// <param name="onClose"> The event executed on use close the ad. </param>
        /// <returns> Return true if the ad is showed </returns>
        bool ShowInterstitial(string placementName = null, Action onClose = null);
        /// <summary>
        /// To show RewardedVideo with placement name.
        /// </summary>
        /// <param name="placementName"> The placement name what we will show to understand the current ad. </param>
        /// <param name="onCompleted"> The event is executed when user end all ad and on rewarded statue. </param>
        /// <param name="onClose"> The event executed on use close the ad. </param>
        /// <returns> Return true if the ad is showed </returns>
        bool ShowRewardedVideo(string placementName = null, Action onCompleted = null, Action onClose = null);
    }
}