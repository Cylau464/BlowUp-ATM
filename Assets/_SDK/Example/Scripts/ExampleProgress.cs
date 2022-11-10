using UnityEngine;
using UnityEngine.UI;
using apps;

public class ExampleProgress : MonoBehaviour
{
    public int currentLevel;
    public Text textLevel;

    protected void OnEnable()
    {
        currentLevel = PlayerPrefs.GetInt("level id");
    }

    public void MakeStart()
    {
        EventsLogger.ProgressStartEvent(new ProgressStartInfo(currentLevel, "Level" + currentLevel, currentLevel, "easy", 0, false, "normal", "classic"));
        ADSManager.AutoShowInterstitial("MakeStart");
    }

    public void MakeWin()
    {
        EventsLogger.ProgressCompletedEvent(new ProgressCompletedInfo(currentLevel, "Level" + currentLevel, currentLevel, "easy", 0, false, "normal", "classic", "100", 20, 0));
        ADSManager.AutoShowInterstitial("MakeLose");
    }

    public void MakeLose()
    {
        EventsLogger.ProgressFailedEvent(new ProgressFailedInfo(currentLevel, "Level" + currentLevel, currentLevel, "easy", 0, false, "normal", "classic", "22", "spikes", 100, 0));
        ADSManager.AutoShowInterstitial("MakeWin");
        currentLevel++;
        PlayerPrefs.SetInt("level id", currentLevel);
    }
}
