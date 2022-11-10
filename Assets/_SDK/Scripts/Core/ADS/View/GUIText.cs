using UnityEngine;

namespace apps.adsview
{
    internal class GUIText : GUIView
    {
        private string _text;
        private GUIStyle _style;

        internal override void Initialize(GUIContent content, Rect rect)
        {
            base.Initialize(content, rect);

            _style = new GUIStyle();
            _style.alignment = TextAnchor.MiddleCenter;
        }

        internal void Initialize(GUIContent content, Rect rect, string text)
        {
            isEnable = true;

            _content = content;

            _rect = rect;

            _text = text;

            _style = new GUIStyle();
            _style.alignment = TextAnchor.MiddleCenter;
        }


        internal void SetText(string text)
        {
            _text = text;
        }

        protected override void Draw()
        {
            _style.fontSize = GUI.skin.label.fontSize;
            GUI.Label(_rect, _text, _style);
        }
    }
}