using apps.exception;
using System.Collections.Generic;

namespace apps
{
    public enum SessionStatue { Started, Completed }
    public static class EventsLogger
    {
        private static Stack<IEvent> _eventLoggers = new Stack<IEvent>();
        private static List<string> _activeStatues = new List<string>();
        public static bool isHasLoggers => _eventLoggers.Count != 0;
        public static int totalLoggers => _eventLoggers.Count;

        /// <summary>
        /// To add some event loggers.
        /// </summary>
        /// <param name="eventLogger"> Is object of type IEvent. </param>
        public static void AddEvent(IEvent eventLogger)
        {
            if (eventLogger == null) throw new System.ArgumentNullException();

            _eventLoggers.Push(eventLogger);
        }

        /// <summary>
        /// To add array of events logger.
        /// </summary>
        public static void AddEvents(IEvent[] eventLoggers)
        {
            if (eventLoggers == null) throw new System.ArgumentNullException();
            
            for (int i = 0; i < eventLoggers.Length; i++)
            {
                if (eventLoggers[i] == null)
                    throw new ArrayHasObjectNullException();

                EventsLogger._eventLoggers.Push(eventLoggers[i]);
            }
        }

        /// <summary>
        /// Clear all events.
        /// </summary>
        public static void ClearEvents()
        {
            _eventLoggers.Clear();
        }

        /// <summary>
        /// To send a custom event.
        /// </summary>
        /// <param name="eventName"> The event name that we will send. </param>
        /// <param name="value"> The value of event, example: The score.</param>
        public static void CustomEvent(string eventName)
        {
            foreach (IEvent logger in _eventLoggers)
            {
                logger.CustomEvent(eventName);
            }
        }

        /// <summary>
        /// To send a custom event.
        /// </summary>
        /// <param name="eventName"> The event name that we will send. </param>
        /// <param name="value"> The value of event, example: The score.</param>
        public static void CustomEvent(string eventName, Dictionary<string, object> values)
        {
            foreach (IEvent logger in _eventLoggers)
            {
                logger.CustomEvent(eventName, values);
            }
        }

        /// <summary>
        /// To send a session event.
        /// </summary>
        /// <param name="sessionName"> The session name that we will send. </param>
        /// <param name="statue"> The session statue that we will send it can be started or completed. </param>
        public static void SessionEvent(string sessionName, SessionStatue statue)
        {
            switch (statue)
            {
                case SessionStatue.Started:
                    _activeStatues.Add(sessionName);
                    break;
                case SessionStatue.Completed:
                    _activeStatues.Remove(sessionName);
                    break;
            }

            foreach (IEvent logger in _eventLoggers)
            {
                logger.SessionEvent(sessionName, statue);
            }
        }

        public static void CompleteAllSessions()
        {
            foreach (string session in _activeStatues)
            {
                foreach (IEvent logger in _eventLoggers)
                {
                    logger.SessionEvent(session, SessionStatue.Completed);
                }
            }
            _activeStatues.Clear();
        }

        /// <summary>
        /// To send a ProgressEvent event.
        /// </summary>
        public static void ProgressStartEvent(ProgressStartInfo progressInfo)
        {
            foreach (IEvent logger in _eventLoggers)
            {
                logger.ProgressStartedEvent(progressInfo);
            }
        }

        /// <summary>
        /// To send a ProgressEvent event.
        /// </summary>
        public static void ProgressFailedEvent(ProgressFailedInfo progressInfo)
        {
            foreach (IEvent logger in _eventLoggers)
            {
                logger.ProgressFailedEvent(progressInfo);
            }
        }
        
        /// <summary>
        /// To send a ProgressEvent event.
        /// </summary>
        public static void ProgressCompletedEvent(ProgressCompletedInfo progressInfo)
        {
            foreach (IEvent logger in _eventLoggers)
            {
                logger.ProgressCompletedEvent(progressInfo);
            }
        }

        /// <summary>
        /// To send some error events.
        /// </summary>
        /// <param name="severity"> The error type. </param>
        /// <param name="message"> The message content in the error. </param>
        public static void ErrorEvent(ErrorSeverity severity, string message)
        {
            foreach (IEvent logger in _eventLoggers)
            {
                logger.ErrorEvent(severity, message);
            }
        }

        /// <summary>
        /// To send some products buying in the game.
        /// </summary>
        /// <param name="productIAPID"> product ID of the IAP. </param>
        /// <param name="price"> The price that spended on this product. </param>
        public static void IAPEvent(InAppInfo info)
        {
            foreach (IEvent logger in _eventLoggers)
            {
                logger.IAPEvent(info);
            }
        }

        /// <summary>
        /// Send events on show some ADS.
        /// </summary>
        /// <param name="adType"> The ads type example Rewardedvideo. </param>
        /// <param name="placementName"> The placement name of this ad. </param>
        public static void AdEvent(EventADSName eventADSName, AdType adType, string placement, EventADSResult result)
        {
            foreach (IEvent logger in _eventLoggers)
            {
                logger.AdEvent(eventADSName, adType, placement, result);
            }
        }
    }
}
