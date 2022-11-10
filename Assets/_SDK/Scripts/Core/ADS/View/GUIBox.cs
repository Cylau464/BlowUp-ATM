using UnityEngine;

namespace apps.adsview
{
    internal class GUIBox : GUIView
    {
        protected override void Draw()
        {
            GUI.Box(_rect, _content);
        }
    }
}
