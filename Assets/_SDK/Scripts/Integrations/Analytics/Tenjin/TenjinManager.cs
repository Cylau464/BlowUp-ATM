using System;
using System.Collections.Generic;
using UnityEngine;

namespace apps
{
    public enum TrackingState { Undefined, Unknown, Restricted, Denied, Authorized }
    public class TenjinManager : IApplicationPause, IEvent
    {
        public static TrackingState TrackingState { get; private set; } = TrackingState.Undefined;
        public static Action<TrackingState> CompletionTracking;


        private BaseTenjin Instance;

        public static bool RequireATT
        {
            get
            {
#if UNITY_IOS
                if (new System.Version(UnityEngine.iOS.Device.systemVersion).CompareTo(new System.Version("14.0")) >= 0)
                    return true;

                return false;
#else
                return true;
#endif
            }
        }

        private string _key;

        public TenjinManager(string key)
        {
            _key = key;
            TenjinConnect();
        }

        public void TenjinConnect()
        {
            Instance = Tenjin.getInstance(_key);

#if UNITY_ANDROID

            Instance.SetAppStoreType(AppStoreType.googleplay);
            Instance.Connect();

            TrackingState = TrackingState.Authorized;
            CompletionTracking?.Invoke(TrackingState);

#elif UNITY_IOS
            // instance.RegisterAppForAdNetworkAttribution();
            float iOSVersion = float.Parse(UnityEngine.iOS.Device.systemVersion);
            if (iOSVersion == 14.0)
            {
                // Tenjin wrapper for requestTrackingAuthorization
                Instance.RequestTrackingAuthorizationWithCompletionHandler((status) => {
                    Debug.Log("===> App Tracking Transparency Authorization Status: " + status);
                    switch (status)
                    {
                        case 0:
                            Debug.Log("ATTrackingManagerAuthorizationStatusNotDetermined case");
                            Debug.Log("Not Determined");
                            Debug.Log("Unknown consent");
                            TrackingState = TrackingState.Unknown;
                            break;
                        case 1:
                            Debug.Log("ATTrackingManagerAuthorizationStatusRestricted case");
                            Debug.Log(@"Restricted");
                            Debug.Log(@"Device has an MDM solution applied");
                            TrackingState = TrackingState.Restricted;
                            break;
                        case 2:
                            Debug.Log("ATTrackingManagerAuthorizationStatusDenied case");
                            Debug.Log("Denied");
                            Debug.Log("Denied consent");
                            TrackingState = TrackingState.Denied;
                            break;
                        case 3:
                            Debug.Log("ATTrackingManagerAuthorizationStatusAuthorized case");
                            Debug.Log("Authorized");
                            Debug.Log("Granted consent");
                            TrackingState = TrackingState.Authorized;
                            Instance.Connect();
                            break;
                        default:
                            TrackingState = TrackingState.Unknown;
                            Debug.Log("Unknown");
                            break;
                    }
                });
            }
            else
            {
                TrackingState = TrackingState.Authorized;
                Instance.Connect();
            }

            CompletionTracking?.Invoke(TrackingState);
#endif
        }

        void IApplicationPause.OnApplicationPause(bool pauseStatus)
        {
            if (!pauseStatus)
            {
                TenjinConnect();
            }
        }

        public void CustomEvent(string eventName)
        {
            //Instance.SendEvent(eventName.Replace(":", ""));
        }

        public void CustomEvent(string eventName, Dictionary<string, object> dictionary)
        {
            //Instance.SendEvent(eventName.Replace(":", ""));
        }

        public void SessionEvent(string sessionName, SessionStatue statue)
        {
            //Instance.SendEvent(sessionName, statue.ToString());
        }

        public void ProgressStartedEvent(ProgressStartInfo progressInfo)
        {
            //Instance.SendEvent($"LevelStarte{progressInfo.playerLevel}");
        }

        public void ProgressFailedEvent(ProgressFailedInfo progressInfo)
        {
            //Instance.SendEvent($"LevelFailed{progressInfo.playerLevel}");
        }

        public void ProgressCompletedEvent(ProgressCompletedInfo progressInfo)
        {
            //Instance.SendEvent($"LevelConmpleted{progressInfo.playerLevel}");
        }

        public void ErrorEvent(ErrorSeverity severity, string message)
        {
            //Instance.SendEvent($"Error{severity}, {message}");
        }

        public void IAPEvent(InAppInfo info)
        {
#if UNITY_ANDROID
            Instance.Transaction(info.InAppID, info.Currency, 1, info.Price, null, info.Receipt, info.SignatureAndTransactionID);
#elif UNITY_IOS
            Instance.Transaction(info.InAppID, info.Currency, 1, info.Price, info.SignatureAndTransactionID, info.Receipt, null);
#endif
        }

        public void AdEvent(EventADSName eventADSName, AdType adType, string placement, EventADSResult result)
        {
            //if (result == EventADSResult.start)
            //{
            //    Instance.SendEvent($"{eventADSName}{adType}{placement}");
            //}
        }
    }
}