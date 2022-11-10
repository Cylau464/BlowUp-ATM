using apps;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ExampleScript : MonoBehaviour
{
    public InputField inputField;

    #region rewards
    public void ShowRewardedVideo()
    {
        ADSManager.ShowRewardedVideo("Coins3X", OnCompletedRewardedVideo);
    }

    void OnCompletedRewardedVideo()
    {
        inputField.text = "OnCompletedRewardedVideo";
    }
    #endregion

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void ShowInterstitial()
    {
        ADSManager.ShowInterstitial("OnFinished");
    }
    
    public void BuyProduct()
    {
        EventsLogger.IAPEvent(new InAppInfo("rmvads", "US", 1.99f, "Removead"));
    }

    public void ShowBanner()
    {
        ADSManager.DisplayBanner();
    }

    public void HideBanner()
    {
        ADSManager.HideBanner();
    }

    public void SendEvent()
    {
        EventsLogger.CustomEvent(inputField.text);
    }

    public void SendEvent(string eventName)
    {
        EventsLogger.CustomEvent(eventName);
    }
}