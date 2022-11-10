using apps.analytics;
using UnityEngine;

namespace apps
{
    public enum ShowADSType { Simulator, Debug, Non }

    [CreateAssetMenu(fileName = "AppsSettings", menuName = "AppsSettings", order = 1)]
    public class AppsSettings : ScriptableObject
    {
        public readonly static string globalDirectoryPath = "Assets/_SDK/Resources/";

        public bool autoInitialize = true;

        public bool integrateFacebook = true;
        public string appLabels = "";
        public string appFacebookID = "Entry Facebook ID...";
        public string clientTokens = "";


        public bool integrateADS = false;
        public ShowADSType showADSType = ShowADSType.Simulator;
        public ADSInfo adsInfo;

        public bool integrateGameAnalytics = true;
        public bool integrateAppMetrica = true;
        public AppMetricaInfo appMetricaInfo;

        public bool integrateTenjin = true;
        public string tenjinApiKey = "YAZ1QBH6S2WVW1C2DRSSOF1NNVHDSMSH";

        public bool debugMode = true;
    }
}
