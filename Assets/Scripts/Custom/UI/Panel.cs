using UnityEngine;

namespace main.ui
{
    public abstract class Panel : MonoBehaviour, IPanel
    {
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}