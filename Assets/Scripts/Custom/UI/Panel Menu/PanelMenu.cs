using UnityEngine;

namespace main.ui
{
    public class PanelMenu : Panel
    {
        [SerializeField] protected MainCanvasManager _mainCanvasManager;
        [SerializeField] protected LevelsProgress _levelsProgress;

        public void StartGame()
        {
            _mainCanvasManager.StartGame();
        }

        public override void Show()
        {
            _levelsProgress.Initialize();
        }

        protected void OnValidate()
        {
            if (_mainCanvasManager == null)
                _mainCanvasManager = GetComponentInParent<MainCanvasManager>();
        }
    }
}
