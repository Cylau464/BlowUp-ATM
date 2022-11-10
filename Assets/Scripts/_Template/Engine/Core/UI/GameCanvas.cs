using System.Collections.Generic;
using UnityEngine;

namespace core.ui
{
    public abstract class GameCanvas : MonoBehaviour
    {
        private List<IPanel> _panels = new List<IPanel>();
        public IPanel activatePanel { get; private set; }

        protected void ShowPanel(IPanel panel)
        {
            if (panel == null) throw new System.NullReferenceException();

            panel.Show();
            _panels.Add(panel);
        }

        protected void HidePanel(IPanel panel)
        {
            if (panel == null) throw new System.NullReferenceException();

            panel.Hide();
            _panels.Add(panel);
        }

        public void SwitchPanel(IPanel switchToPanel)
        {
            if (switchToPanel == null) throw new System.NullReferenceException();

            if (activatePanel != null) HidePanel(activatePanel);
            activatePanel = switchToPanel;
            ShowPanel(activatePanel);
        }

        public void ShowLastPanel()
        {
            if (activatePanel != null)
            {
                ShowPanel(activatePanel);
            }
        }

        protected void ClearAllPanels()
        {
            _panels.RemoveAll(item => { if (item != null && !item.Equals(null)) item.Hide(); return true; });
        }
    }
}