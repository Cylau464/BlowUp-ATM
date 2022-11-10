using core.ui;
using engine;
using UnityEngine;

namespace main.ui
{
    public class MainCanvasManager : GameCanvas, ILevelCompleted, ILevelFailed
    {
        [SerializeField] private GameManager _gameManager;

        [Header("Panels")]
        [SerializeField] private Panel _menuPanel;
        [SerializeField] private Panel _playPanel;
        [SerializeField] private Panel _losePanel;
        [SerializeField] private Panel _winPanel;

        [Header("Settings")]
        [SerializeField] private float _timeAndShowWinLosePanel = 2f;

        // Start is called before the first frame update
        protected void OnEnable()
        {
            LevelStatueCompleted.Subscribe(this);
            LevelStatueFailed.Subscribe(this);

            ShowLastPanel();
            SwitchPanel(_menuPanel);
        }

        public void StartGame()
        {
            _gameManager.MakeStarted();
            OnStart();
        }

        private void OnStart()
        {
            SwitchPanel(_playPanel);
        }

        public void LevelCompleted()
        {
            StartCoroutine(WaitAndShowWin());
        }

        public void LevelFailed()
        {
            StartCoroutine(WaitAndShowLose());
        }

        #region coroutine
        private System.Collections.IEnumerator WaitAndShowLose()
        {
            yield return new WaitForSeconds(_timeAndShowWinLosePanel);
            SwitchPanel(_losePanel);
        }

        private System.Collections.IEnumerator WaitAndShowWin()
        {
            yield return new WaitForSeconds(_timeAndShowWinLosePanel);
            SwitchPanel(_winPanel);
        }
        #endregion

        public void Next()
        {
            ReloadScene();
        }

        public void ReloadScene()
        {
            GameScenes.ReloadScene();
        }

#if UNITY_EDITOR
        protected void OnValidate()
        {
            if (_gameManager == null)
                _gameManager = editor.EditorManager.FindScenesComponent<GameManager>();
        }
#endif
    }
}
