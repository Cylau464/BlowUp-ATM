using System.Collections.Generic;
using UnityEngine;

namespace apps.adsview
{
    internal class Window
    {
        internal bool isEnable { get; private set; }

        private GUIText _text;
        internal GameObject gameObject { get; private set; }


        internal Window(GameObject windowGameObject)
        {
            this.gameObject = windowGameObject ?? throw new System.ArgumentNullException("The GameObject is has null value.");
            Object.DontDestroyOnLoad(this.gameObject);

            _text = (GUIText)this.gameObject.AddComponent(typeof(GUIText));
            _text.Initialize(new GUIContent(),
                new Rect(
                    Screen.width / 2,
                    Screen.height / 2,
                    0,
                    0
                )
                );

            HideWindow();
        }

        internal void ShowWindow(string placementName = "")
        {
            _text.SetText(placementName);
            isEnable = true;
            gameObject.SetActive(true);
        }

        internal void HideWindow()
        {
            _text.SetText("");
            isEnable = false;
            gameObject.SetActive(false);
        }
    }
}
