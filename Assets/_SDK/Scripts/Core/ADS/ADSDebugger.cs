using System;
using UnityEngine;

namespace apps
{

    public class ADSDebugger : IADS
    {
        public bool isDebuging { get; set; }

        public bool useBanner { get; set; }
        public bool useInterstitial { get; set; }
        public bool useRewardedVideo { get; set; }

        public ADSDebugger(string key, bool isDebuging)
        {
            this.isDebuging = isDebuging;
            if (isDebuging) Debug.Log("The ADS is intialized with key: " + key);
        }

        public void LoadBanner()
        {
            if (isDebuging) Debug.Log("The Banner ad is Loaded.");
        }

        public void DisplayBanner()
        {
            if (isDebuging) Debug.Log("The Banner ad is Displayed.");
        }

        public void HideBanner()
        {
            if (isDebuging) Debug.Log("The Banner ads is Hided.");
        }
        public void CreateBanner()
        {
            if (isDebuging) Debug.Log("The Banner ad is Created.");
        }

        public void DestroyBanner()
        {
            if (isDebuging) Debug.Log("The Banner ad is Destroyed.");
        }

        public void LoadInterstitial()
        {
            if (isDebuging) Debug.Log("The Interstitial ad is Loaded.");
        }

        public bool IsInterstitialAvailable()
        {
            return true;
        }

        public bool ShowInterstitial(string placementName = null, Action onClose = null)
        {
            if (isDebuging) Debug.Log("Show Interstitial ad.");
            return true;
        }

        public void LoadRewardedVideo()
        {
            if (isDebuging) Debug.Log("The RewardsVideo ad is Loaded.");
        }

        public bool IsRewardedideoAvailable()
        {
            return true;
        }

        public bool ShowRewardedVideo(string placementName = null, Action onCompleted = null, Action onClose = null)
        {
            if (isDebuging)
            {
                Debug.Log("Show RewardsVideo ad.");
                Debug.Log("RewardsVideo ad is completed.");
            }
            return true;
        }
    }
}
