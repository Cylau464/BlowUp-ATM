using System.Collections.Generic;
using UnityEngine;

namespace apps
{
    public class EventsDebug : IEvent
    {
        public EventsDebug()
        {
            Initialize();
        }

        protected void Initialize()
        {
            Debug.Log("Events is initialized");
        }

        public void CustomEvent(string eventName)
        {
            Debug.Log("Design-Event, Event name: " + eventName.Replace(':', '/'));
        }

        public void CustomEvent(string eventName, Dictionary<string, object> dictionary)
        {
            Debug.Log("Design-Event, Event name: " + eventName.Replace(':', '/') + ", values: " + JsonUtility.ToJson(dictionary));
        }

        public void SessionEvent(string sessionName, SessionStatue statue)
        {
            Debug.Log("Session-Event, session name: " + sessionName.Replace(':', '/') + ", statue: " + statue);
        }

        public void AdEvent(AdType adType, string placementName)
        {
            Debug.Log("Ads-Event, Type: " + adType + ", Placement: " + placementName);
        }

        public void ErrorEvent(ErrorSeverity severity, string message)
        {
            Debug.Log("Error-Event, Severity: " + severity + ", message: " + message);
        }

        public void ProgressStartedEvent(ProgressStartInfo progressInfo)
        {
            Debug.Log("Progress-Event, ProgressionStatus: Started, progression: " + progressInfo.playerLevel);
        }

        public void ProgressFailedEvent(ProgressFailedInfo progressInfo)
        {
            Debug.Log("Progress-Event, ProgressionStatus: Failed, progression: " + progressInfo.playerLevel + ", Reason: " + progressInfo.reason);
        }

        public void ProgressCompletedEvent(ProgressCompletedInfo progressInfo)
        {
            Debug.Log("Progress-Event, ProgressionStatus: Completed, progression: " + progressInfo.playerLevel);
        }

        public void IAPEvent(InAppInfo info)
        {
            Debug.Log("IAP-Event, inappID: " + info.InAppID + ", currency: " + info.Currency + ", price: " + info.Price + ", inapp_type: " + info.InAppType);
        }

        public void AdEvent(EventADSName eventADSName, AdType adType, string placement, EventADSResult result)
        {
            Debug.Log("Ad-Event, eventADSName: " + eventADSName + ", adType: " + adType + ", placement: " + placement + ", result: " + result);
        }
    }
}