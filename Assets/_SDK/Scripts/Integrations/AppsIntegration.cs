using apps.analytics;
using System.Collections.Generic;
using UnityEngine;

namespace apps
{
    public static class AppsIntegration
    {
        public readonly static string s_AppSettingsName = "AppsSettings";
        private static List<object> s_Applications = new List<object>();
        private static List<IApplicationPause> s_ApplicationsPause = new List<IApplicationPause>();
        private static bool _isInited;
        private static GameObject _objectHelper;

        [RuntimeInitializeOnLoadMethod]
        public static void AutoInitialize()
        {
            AppsSettings settings = Resources.Load<AppsSettings>(s_AppSettingsName);
            if (settings.autoInitialize) Initialize(settings);
        }

        public static void Initialize()
        {
            Initialize(Resources.Load<AppsSettings>(s_AppSettingsName));
        }

        public static void Initialize(AppsSettings settings)
        {
            if (_isInited == true) return;

            if (settings == null) throw new System.NullReferenceException("AppsSettings not found in resource folder!");

            InstintiateObjectHelper();

            if (settings.integrateADS)
            {
                IADS adsMaker = null;
#if Support_Applovin
                ADSInfo adsInfo = settings.adsInfo;
                adsMaker = new ad.AppLovinADS(adsInfo.appLovinInfo, adsInfo.useBanner, adsInfo.useInterstitial, adsInfo.useRewardedVideo);
#elif Support_IronSource
                ADSInfo adsInfo = settings.adsInfo;
                adsMaker = new ad.IronSourceADS(adsInfo.ironSourceInfo, adsInfo.useBanner, adsInfo.useInterstitial, adsInfo.useRewardedVideo);
#else
                switch (settings.showADSType)
                {
                    case ShowADSType.Simulator:
                        adsMaker = new ADSSimulator("####### Simulator ######", settings.adsInfo.useBanner, settings.adsInfo.useInterstitial, settings.adsInfo.useRewardedVideo);
                        break;
                    case ShowADSType.Debug:
                    case ShowADSType.Non:
                        adsMaker = new ADSDebugger("####### Debuger ######", settings.showADSType == ShowADSType.Debug);
                        break;
                }
#endif
                ADSManager.Initialize(adsMaker, true, settings.adsInfo.showInterstitialEvery);
                s_Applications.Add(adsMaker);
            }

            if (settings.integrateFacebook)
            {
                FacebookApp.InitFacebookApp();
            }

            if (settings.integrateTenjin)
            {
                TenjinManager tenjin = new TenjinManager(settings.tenjinApiKey);
                EventsLogger.AddEvent(tenjin);
                s_ApplicationsPause.Add(tenjin);
            }

            if (settings.integrateGameAnalytics)
            {
                GameAnalyticsEvents GA = new GameAnalyticsEvents();

                _objectHelper.AddComponent(typeof(GameAnalyticsSDK.Events.GA_SpecialEvents));
                _objectHelper.AddComponent(typeof(GameAnalyticsSDK.GameAnalytics));

                EventsLogger.AddEvent(GA);
                s_Applications.Add(GA);
            }

            if (settings.integrateAppMetrica)
            {
                AppMetricaEvents appMetrica = new AppMetricaEvents(settings.appMetricaInfo);

                EventsLogger.AddEvent(appMetrica);
                s_ApplicationsPause.Add(appMetrica);
                s_Applications.Add(appMetrica);
            }
            
            if (settings.debugMode)
            {
                EventsDebug debug = new EventsDebug();
                EventsLogger.AddEvent(debug);
                s_Applications.Add(debug);
            }

            Debug.Log("The apps is initialized...");
            _isInited = true;
        }

        private static void InstintiateObjectHelper()
        {
            _objectHelper = new GameObject("Integrations Helper");
            _objectHelper.AddComponent(typeof(IntegrationHelper));
            Object.DontDestroyOnLoad(_objectHelper);
        }

        public static void OnApplicationPause(bool pause)
        {
            foreach (IApplicationPause Application in s_ApplicationsPause)
            {
                if (Application != null && !Application.Equals(null)) Application.OnApplicationPause(pause);
            }
        }
    }
}
