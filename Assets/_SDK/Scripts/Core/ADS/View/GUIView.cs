using UnityEngine;

namespace apps.adsview
{
    internal abstract class GUIView : MonoBehaviour
    {
        internal bool isEnable { get; set; }
        public readonly static float heightScaling = Screen.height / 1920.0f;

        protected GUIContent _content;
        protected Rect _rect;

        internal virtual void Initialize(GUIContent content, Rect rect)
        {
            isEnable = true;

            _content = content;

            _rect = rect;
        }

        protected void OnGUI()
        {
            if (!isEnable)
                return;

            GUI.skin.button.fontSize =
                GUI.skin.box.fontSize =
                GUI.skin.label.fontSize =
                (int)(35 * heightScaling);

            Draw();
        }

        protected abstract void Draw();

    }
}
