using UnityEngine;

namespace apps.adsview
{
    internal class GUIButton : GUIView
    {
        private System.Action _onClick;

        internal void Initialize(GUIContent content, Rect rect, System.Action onClick)
        {
            isEnable = true;

            _content = content;

            _rect = rect;

            _onClick = onClick;
        }

        protected override void Draw()
        {
            if (GUI.Button(_rect, _content))
            {
                _onClick?.Invoke();
            }
        }
    }
}
