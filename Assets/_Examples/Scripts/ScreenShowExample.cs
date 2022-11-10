using engine;
using UnityEngine;

public class ScreenShowExample : MonoBehaviour
{
    protected void OnGUI()
    {
        GUIDisplay.MakeLabel("Level playing: " + GameManager.isPlaying, 0, Color.blue, GUISide.Left);
        GUIDisplay.MakeLabel("Level lost: " + GameManager.isFailed, 1, Color.blue, GUISide.Left);
        GUIDisplay.MakeLabel("Level win: " + GameManager.isCompleted, 2);
    }
}