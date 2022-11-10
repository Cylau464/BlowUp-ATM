using Facebook.Unity;

namespace apps.analytics
{
    public static class FacebookApp
    {
        public static void InitFacebookApp()
        {
            if (FB.IsInitialized == true)
            {
                CallEvents();
            }
            else
            {
                FB.Init(() =>
                {
                    CallEvents();
                });
            }
        }

        public static void CallEvents()
        {
            FB.ActivateApp();
            FB.LogAppEvent(AppEventName.ActivatedApp);
            FB.Mobile.SetAdvertiserIDCollectionEnabled(true);
        }
    }
}