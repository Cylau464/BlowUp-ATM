using UnityEngine;

namespace main.level
{
    [System.Serializable]
    public struct SceneContainer
    {
        public GameManager gameManager;

        public void OnValidate()
        {
#if UNITY_EDITOR
            if (gameManager == null)
                gameManager = editor.EditorManager.FindScenesComponent<GameManager>();
#endif
        }
    }

    public class LevelsManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] protected GameSettings _settings;
        [SerializeField] protected Transform _levelsContents;

        [Header("Container")]
        [SerializeField] protected SceneContainer _sceneContainer;

        public int totalLevels { get { return _settings.levelContainer.totalLevels; } }

        public static float[] progressCheckPoints = new float[]
        {
            .4f,
            .65f,
            .85f
        };
        public static Level currentLevel { get; private set; }
        public static bool isLevelLoaded { get; private set; } = false;

        protected void OnEnable()
        {
            if (_settings.isTestingMode)
            {
                currentLevel = FindObjectOfType<Level>();
                if (currentLevel != null)
                {
                    DefineCurrentLevel(currentLevel);
                    return;
                }
                else
                    MakeLoadLevel();
            }
            else
                MakeLoadLevel();

            GameManager.OnCompleted += OnCompleteLevel;
        }

        private void OnDisable()
        {
            GameManager.OnCompleted -= OnCompleteLevel;
        }

        private void MakeLoadLevel()
        {
            currentLevel = _settings.levelContainer.currentLevel;
            DefineCurrentLevel(Instantiate(currentLevel, _levelsContents));
        }

        private void OnCompleteLevel()
        {
            currentLevel.levelInfo.LoopLevel++;
        }

        protected void DefineCurrentLevel(Level level)
        {
            currentLevel = level ?? throw new System.ArgumentNullException();

            isLevelLoaded = true;
            currentLevel.Initialize(this, _sceneContainer);
        }

        protected void OnDestroy()
        {
            currentLevel = null;
            isLevelLoaded = false;
        }

        protected void OnValidate()
        {
            _sceneContainer.OnValidate();
        }
    }
}
