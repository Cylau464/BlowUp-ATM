using System;
using UnityEngine;
using apps.adsview;
using apps.exception;

namespace apps
{
    public class ADSSimulator : IADS
    {
        public readonly static float widthScaling = Screen.width / 1080.0f;
        public readonly static float heightScaling = Screen.height / 1920.0f;

        protected Action _onCompletedRewardsVideo = null;
        protected Action _onClosedRewardsVideo = null;
        protected Action _onClosedInterstitial = null;

        private bool _useBanner;
        public bool useBanner
        {
            get { return _useBanner; }
            set
            {
                if (value == true)
                    bannerIsLoaded = true;

                _useBanner = value;
            }
        }

        private bool _useInterstitial;
        public bool useInterstitial
        {
            get { return _useInterstitial; }
            set
            {
                if (value == true)
                    interstitialIsLoaded = true;

                _useInterstitial = value;
            }
        }

        private bool _useRewardsVideo;
        public bool useRewardedVideo
        {
            get { return _useRewardsVideo; }
            set
            {
                if (value == true)
                    rewardsVideoIsLoaded = true;

                _useRewardsVideo = value;
            }
        }

        public bool bannerIsLoaded { get; private set; } = false;
        public bool interstitialIsLoaded { get; private set; } = false;
        public bool rewardsVideoIsLoaded { get; private set; } = false;

        private Window _bannerWindow;
        private Window _interstitialWindow;
        private Window _rewardsVideoWindow;

        public bool bannerIsDisplaying => _bannerWindow.isEnable;
        public bool interstitialIsDisplaying => _interstitialWindow.isEnable;
        public bool rewardsVideoIsDisplaying => _rewardsVideoWindow.isEnable;

        public ADSSimulator(string key, bool useBanner, bool useInterstitial, bool useRewardsVideo)
        {
            if (key == null || key.CompareTo("") == 0)
                throw new ArgumentEmptryOrNullException();

            _useBanner = useBanner;
            _useInterstitial = useInterstitial;
            _useRewardsVideo = useRewardsVideo;

            Intialize(key);
        }

        private void Intialize(string key)
        {
            Debug.Log("The ADS is intialized with key: " + key);

            CreateBannerWindow();
            CreateInterstitialWindow();
            CreateRewardsVideoWindow();

            LoadBanner();
            LoadInterstitial();
            LoadRewardedVideo();
        }

        #region banner
        private void CreateBannerWindow()
        {
            if (_bannerWindow != null)
                return;

            GameObject bannerObject = new GameObject("Banner");
            _bannerWindow = new Window(bannerObject);

            GUIBox bannerView = (GUIBox)bannerObject.AddComponent(typeof(GUIBox));
            bannerView.Initialize(
                new GUIContent("Banner"),
                new Rect(
                    0,
                    Screen.height - 80 * heightScaling,
                    Screen.width,
                    80 * heightScaling
                )
                );
        }

        public void LoadBanner()
        {
            if (!_useBanner)
                return;

            bannerIsLoaded = true;
            Debug.Log("The Banner ad is Loaded.");
        }

        public void DisplayBanner()
        {
            if (!bannerIsLoaded) return;

            _bannerWindow.ShowWindow();
        }

        public void HideBanner()
        {
            _bannerWindow.HideWindow();
        }

        public void CreateBanner()
        {
            CreateBannerWindow();
        }

        public void DestroyBanner()
        {
            UnityEngine.Object.Destroy(_bannerWindow.gameObject);
        }
        #endregion

        #region interstitial
        private void CreateInterstitialWindow()
        {
            if (_interstitialWindow != null)
                return;

            GameObject interstitialObject = new GameObject("Interstitial");
            _interstitialWindow = new Window(interstitialObject);

            /// Create background Interstitial
            GUIBox backgroundView = (GUIBox)interstitialObject.AddComponent(typeof(GUIBox));
            backgroundView.Initialize(
                new GUIContent(""),
                new Rect(0, 0, Screen.width, Screen.height)
                );

            /// Create close button Interstitial
            GUIButton buttonView = (GUIButton)interstitialObject.AddComponent(typeof(GUIButton));
            buttonView.Initialize(
                new GUIContent("X"),
                new Rect(
                    Screen.width - 120 * widthScaling,
                    20 * heightScaling,
                    100 * widthScaling,
                    100 * heightScaling
                    ),
                OnClickInterstitialClose
                );
        }

        public void LoadInterstitial()
        {
            if (!_useInterstitial)
                return;

            interstitialIsLoaded = true;
            Debug.Log("The Interstitial ad is Loaded.");
        }

        public bool IsInterstitialAvailable()
        {
            return _useInterstitial && interstitialIsLoaded;
        }

        public bool ShowInterstitial(string placementName = null, Action onClosed = null)
        {
            if (!IsInterstitialAvailable())
                return false;

            _onClosedInterstitial = onClosed;

            _interstitialWindow.ShowWindow(placementName);
            interstitialIsLoaded = false;
            return true;
        }

        private void OnClickInterstitialClose()
        {
            _interstitialWindow.HideWindow();
            _onClosedInterstitial?.Invoke();
            LoadInterstitial();
        }
        #endregion

        #region rewards video
        private void CreateRewardsVideoWindow()
        {
            if (_rewardsVideoWindow != null)
                return;

            GameObject rewardsVideoObject = new GameObject("RewardsVideo");
            _rewardsVideoWindow = new Window(rewardsVideoObject);

            /// Create background RewardsVideo
            GUIBox backgroundView = (GUIBox)rewardsVideoObject.AddComponent(typeof(GUIBox));
            backgroundView.Initialize(
                new GUIContent(""),
                new Rect(0, 0, Screen.width, Screen.height)
                );

            /// Create cancel button RewardsVideo
            GUIButton buttonView = (GUIButton)rewardsVideoObject.AddComponent(typeof(GUIButton));
            buttonView.Initialize(
                new GUIContent("X"),
                new Rect(
                    Screen.width - 120 * widthScaling,
                    20 * heightScaling,
                    100 * widthScaling,
                    100 * heightScaling
                    ),
                OnClickClosedRewardsVideo
                );

            /// Create completed button RewardsVideo
            GUIButton completedButtonView = (GUIButton)rewardsVideoObject.AddComponent(typeof(GUIButton));
            completedButtonView.Initialize(
                new GUIContent("Completed"),
                new Rect(
                    Screen.width / 2 - 75 * widthScaling,
                    20 * heightScaling,
                    250 * widthScaling,
                    100 * heightScaling
                    ),
                OnCompletedRewardsVideo
                );


        }

        public void LoadRewardedVideo()
        {
            if (!_useRewardsVideo)
                return;

            rewardsVideoIsLoaded = true;
            Debug.Log("The RewardsVideo ad is Loaded.");
        }

        public bool IsRewardedideoAvailable()
        {
            return _useRewardsVideo && rewardsVideoIsLoaded;
        }

        public bool ShowRewardedVideo(string placementName = null, Action onCompleted = null, Action onClosed = null)
        {
            if (!IsRewardedideoAvailable())
                return false;

            _onCompletedRewardsVideo = onCompleted;
            _onClosedRewardsVideo = onClosed;

            _rewardsVideoWindow.ShowWindow(placementName);
            rewardsVideoIsLoaded = false;

            return true;
        }

        private void OnCompletedRewardsVideo()
        {
            _rewardsVideoWindow.HideWindow();
            _onCompletedRewardsVideo?.Invoke();
            LoadRewardedVideo();
        }

        private void OnClickClosedRewardsVideo()
        {
            _rewardsVideoWindow.HideWindow();
            _onClosedRewardsVideo?.Invoke();
            LoadRewardedVideo();
        }
        #endregion
    }
}
