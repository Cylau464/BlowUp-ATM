using GameAnalyticsSDK;
using System.Collections.Generic;

namespace apps.analytics
{
    public class GameAnalyticsEvents : IEvent
    {
        public GameAnalyticsEvents()
        {
            GameAnalytics.Initialize();
        }

        public void CustomEvent(string eventName)
        {
            GameAnalytics.NewDesignEvent(eventName);
        }

        public void CustomEvent(string eventName, Dictionary<string, object> dictionary)
        {
            GameAnalytics.NewDesignEvent(eventName, dictionary);
        }

        public void SessionEvent(string sessionName, SessionStatue statue)
        {
            GameAnalytics.NewDesignEvent(sessionName + ":" + statue);
        }

        public void IAPEvent(string productIAPID, float price)
        {
            GameAnalytics.NewDesignEvent("IAP:" + productIAPID, price);
        }

        public void ProgressStartedEvent(ProgressStartInfo progressInfo)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, $"Level_{progressInfo.playerLevel}");
            CustomEvent($"LevelDesign:Start:Level_{progressInfo.levelName}", progressInfo.ToDictionary());
        }

        public void ProgressFailedEvent(ProgressFailedInfo progressInfo)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, $"Level_{progressInfo.playerLevel}" );
            CustomEvent($"LevelDesign:Fail:Level_{progressInfo.levelName}", progressInfo.ToDictionary());
        }

        public void ProgressCompletedEvent(ProgressCompletedInfo progressInfo)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, $"Level_{progressInfo.playerLevel}");
            CustomEvent($"LevelDesign:Complete:Level_{progressInfo.levelName}", progressInfo.ToDictionary());
        }

        public void ErrorEvent(ErrorSeverity severity, string message)
        {
            GameAnalytics.NewErrorEvent(ConvertToGAErrorSeverity(severity), message);
        }


        public void IAPEvent(InAppInfo info)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            dictionary.Add("inappID", info.InAppID);
            dictionary.Add("currency", info.Currency);
            dictionary.Add("price", info.Price);
            dictionary.Add("inapp_type", info.InAppType);

            GameAnalytics.NewDesignEvent("IAPEvent", dictionary);
        }

        public void AdEvent(EventADSName eventADSName, AdType adType, string placement, EventADSResult result)
        {
            switch (result)
            {
                case EventADSResult.start:
                    GameAnalytics.NewAdEvent(GAAdAction.Show, ConvertToGAAdType(adType), "standard", placement);
                    break;
                case EventADSResult.fail:
                    GameAnalytics.NewAdEvent(GAAdAction.FailedShow, ConvertToGAAdType(adType), "standard", placement);
                    break;
                case EventADSResult.not_available:
                    GameAnalytics.NewAdEvent(GAAdAction.Request, ConvertToGAAdType(adType), "standard", placement);
                    break;
                case EventADSResult.clicked:
                    GameAnalytics.NewAdEvent(GAAdAction.Clicked, ConvertToGAAdType(adType), "standard", placement);
                    break;
                case EventADSResult.watched:
                    GameAnalytics.NewAdEvent(GAAdAction.RewardReceived, ConvertToGAAdType(adType), "standard", placement);
                    break;
            }
        }

        private GAAdType ConvertToGAAdType(AdType adType)
        {
            switch (adType)
            {
                case AdType.interstitial:
                    return GAAdType.Interstitial;
                case AdType.rewarded:
                    return GAAdType.RewardedVideo;
                default:
                    return GAAdType.Undefined;
            }
        }

        private GAErrorSeverity ConvertToGAErrorSeverity(ErrorSeverity severity)
        {
            switch (severity)
            {
                case ErrorSeverity.Error:
                    return GAErrorSeverity.Error;

                case ErrorSeverity.Warning:
                    return GAErrorSeverity.Warning;

                case ErrorSeverity.Info:
                    return GAErrorSeverity.Info;

                case ErrorSeverity.Critical:
                    return GAErrorSeverity.Critical;

                case ErrorSeverity.Debug:
                    return GAErrorSeverity.Debug;

                default:
                    return GAErrorSeverity.Undefined;
            }
        }
    }
}