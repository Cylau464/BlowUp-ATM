namespace apps
{
    [System.Serializable]
    public class ADSInfo
    {
#if Support_Applovin
        public ad.AppLovinInfo appLovinInfo;
#endif
#if Support_IronSource
        public ad.IronSourceInfo ironSourceInfo;
#endif

        [UnityEngine.Header("Banner")]
        public bool useBanner = true;


        [UnityEngine.Header("interstitial")]
        public bool useInterstitial = true;
        public float showInterstitialEvery = 30;

        [UnityEngine.Header("rewardedVideo")]
        public bool useRewardedVideo = true;
    }
}