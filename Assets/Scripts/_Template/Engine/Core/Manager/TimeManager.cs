using UnityEngine;

public static class TimeManager
{
    public static float defaultScale { get; private set; }

    internal static void Initialize()
    {
        defaultScale = Time.timeScale = 1;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public static void PauseGame()
    {
        SetTimeScale(0);
    }

    public static void PauseSlow()
    {
        SetTimeScale(0.0001f);
    }

    public static void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
        Time.fixedDeltaTime = 0.02F * scale;
    }

    public static void ContinueGame()
    {
        Time.timeScale = defaultScale;
    }
}
