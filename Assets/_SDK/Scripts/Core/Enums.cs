namespace apps
{
    public enum ProgressionStatus
    {
        Undefined = 0,
        Start = 1,
        Complete = 2,
        Fail = 3
    }

    public enum BannerPosition
    {
        TOP = 1,
        BOTTOM = 2
    };

    public enum AdType
    {
        rewarded = 2,
        interstitial = 4
    }

    public enum EventADSName
    {
        video_ads_available = 0,
        video_ads_started = 1,
        video_ads_watch = 2
    }

    public enum EventADSResult
    {
        not_available = 0,
        success = 1,
        start = 2,
        clicked = 3,
        fail = 4,
        watched = 5,
        cancel = 6
    }


    public enum ErrorSeverity
    {
        Undefined = 0,
        Debug = 1,
        Info = 2,
        Warning = 3,
        Error = 4,
        Critical = 5
    }

}
