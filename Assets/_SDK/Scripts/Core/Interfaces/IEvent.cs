using System.Collections.Generic;

namespace apps
{
    public struct InAppInfo
    {
        public InAppInfo(string inAppID, string currency, double price, string inAppType)
        {
            InAppID = inAppID;
            Currency = currency;
            Price = price;
            InAppType = inAppType;

            Quantity = "1";
            Receipt = "Null";
            SignatureAndTransactionID = "Null";
        }

        public InAppInfo(string inAppID, string currency, string quantity, double price, string inAppType, string receipt, string signatureAndTransactionID)
        {
            InAppID = inAppID;
            Currency = currency;
            Quantity = quantity;
            Price = price;
            InAppType = inAppType;
            Receipt = receipt;
            SignatureAndTransactionID = signatureAndTransactionID;
        }

        public string InAppID;
        public string Currency;
        public string Quantity;
        public double Price;
        public string InAppType;
        public string Receipt;
        public string SignatureAndTransactionID;
    }

    public class ProgressStartInfo
    {
        public int playerLevel;
        public string levelName;
        public int levelCount;
        public string difficulty;
        public int levelLoop;
        public bool isRandom;
        public string levelType;
        public string gameMode;

        public ProgressStartInfo(int playerLevel, string levelName, int levelCount, string difficulty, int levelLoop, bool isRandom, string levelType, string gameMode)
        {
            this.playerLevel = playerLevel;
            this.levelName = levelName;
            this.levelCount = levelCount;
            this.difficulty = difficulty;
            this.levelLoop = levelLoop;
            this.isRandom = isRandom;
            this.levelType = levelType;
            this.gameMode = gameMode;
        }

        public virtual Dictionary<string, object> ToDictionary()
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();

            keys.Add("level_number", playerLevel);
            keys.Add("level_name", levelName);
            keys.Add("level_count", levelCount);
            keys.Add("level_diff", difficulty);
            keys.Add("level_loop", levelLoop);
            keys.Add("level_random", (isRandom) ? 1 : 0);
            keys.Add("level_type", levelType);
            keys.Add("game_mode", gameMode);

            return keys;
        }
    }
    
    public class ProgressFailedInfo : ProgressStartInfo
    {
        public string time;
        public string reason;
        public int progress;
        public int continueValue;

        public ProgressFailedInfo(int playerLevel, string levelName, int levelCount, string difficulty, int levelLoop, bool isRandom, string levelType, string gameMode,
            string time, string reason, int progress, int continueValue)
            : base(playerLevel, levelName, levelCount, difficulty, levelLoop, isRandom, levelType, gameMode)
        {
            this.time = time;
            this.reason = reason;
            this.progress = progress;
            this.continueValue = continueValue;
        }

        public override Dictionary<string, object> ToDictionary()
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();

            keys.Add("level_number", playerLevel);
            keys.Add("level_name", levelName);
            keys.Add("level_count", levelCount);
            keys.Add("level_diff", difficulty);
            keys.Add("level_loop", levelLoop);
            keys.Add("level_random", (isRandom) ? 1 : 0);
            keys.Add("level_type", levelType);
            keys.Add("game_mode", gameMode);
            keys.Add("time", time);
            keys.Add("reason", reason);
            keys.Add("result", "lose");
            keys.Add("progress", progress);
            keys.Add("continue", continueValue);

            return keys;
        }
    }

    public class ProgressCompletedInfo : ProgressStartInfo
    {
        public string time;
        public int progress;
        public int continueValue;

        public ProgressCompletedInfo(int playerLevel, string levelName, int levelCount, string difficulty, int levelLoop, bool isRandom, string levelType, string gameMode,
            string time, int progress,  int continueValue)
            : base (playerLevel, levelName, levelCount, difficulty, levelLoop, isRandom, levelType, gameMode)
        {
            this.time = time;
            this.progress = progress;
            this.continueValue = continueValue;
        }

        public override Dictionary<string, object> ToDictionary()
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();

            keys.Add("level_number", playerLevel);
            keys.Add("level_name", levelName);
            keys.Add("level_count", levelCount);
            keys.Add("level_diff", difficulty);
            keys.Add("level_loop", levelLoop);
            keys.Add("level_random", (isRandom) ? 1 : 0);
            keys.Add("level_type", levelType);
            keys.Add("game_mode", gameMode);
            keys.Add("time", time);
            keys.Add("result", "win");
            keys.Add("progress", progress);
            keys.Add("continue", continueValue);

            return keys;
        }
    }

    public interface IEvent
    {

        /// <summary>
        /// To send a custom event.
        /// </summary>
        /// <param name="eventName"> The event name that we will send. </param>
        /// <param name="value"> The value of event, example: The score.</param>
        void CustomEvent(string eventName);

        /// <summary>
        /// To send a custom event.
        /// </summary>
        /// <param name="eventName"> The event name that we will send. </param>
        /// <param name="value"> The value of event, example: The score.</param>
        void CustomEvent(string eventName, Dictionary<string, object> dictionary);

        /// <summary>
        /// To send a session event.
        /// </summary>
        /// <param name="sessionName"> The session name that we will send. </param>
        /// <param name="statue"> The session statue that we will send it can be started or completed. </param>
        void SessionEvent(string sessionName, SessionStatue statue);

        /// <summary>
        /// To send a ProgressEvent event.
        /// </summary>
        void ProgressStartedEvent(ProgressStartInfo progressInfo);

        /// <summary>
        /// To send a ProgressEvent event.
        /// </summary>
        void ProgressFailedEvent(ProgressFailedInfo progressInfo);
        
        /// <summary>
        /// To send a ProgressEvent event.
        /// </summary>
        void ProgressCompletedEvent(ProgressCompletedInfo progressInfo);

        /// <summary>
        /// To send some error events.
        /// </summary>
        /// <param name="severity"> The error type. </param>
        /// <param name="message"> The message content in the error. </param>
        void ErrorEvent(ErrorSeverity severity, string message);

        /// <summary>
        /// To send some products buying in the game.
        /// </summary>
        /// <param name="productIAPID"> product ID of the IAP. </param>
        /// <param name="price"> The price that spended on this product. </param>
        void IAPEvent(InAppInfo info);

        /// <summary>
        /// Send events on show some ADS.
        /// </summary>
        /// <param name="adType"> The ads type example Rewardedvideo. </param>
        /// <param name="placementName"> The placement name of this ad. </param>
        void AdEvent(EventADSName eventADSName, AdType adType, string placement, EventADSResult result);
    }
}