using main.level;
using UnityEngine;
using UnityEngine.UI;

namespace main.ui
{
    public class PanelPlay : Panel
    {
        [Header("Text Level")]
        [SerializeField] private LevelsData _levelsData;
        [SerializeField] private Text _textLevel;
        [SerializeField] private Star _starPrefab;
        [SerializeField] private Slider _progressBar;
        [SerializeField] private float _starSpawnYOffset = 40f;

        private Star[] _stars;

        private void OnEnable()
        {
            LevelsManager.currentLevel.OnCollectMoney += OnCoinsUpdate;
        }

        private void OnDisable()
        {
            if (LevelsManager.currentLevel != null)
                LevelsManager.currentLevel.OnCollectMoney -= OnCoinsUpdate;
        }

        private void OnCoinsUpdate(int count)
        {
            _progressBar.value = LevelsManager.currentLevel.LevelProgress;
        }

        protected void Start()
        {
            InitializedTextLevel();
            _progressBar.value = 0f;
            _stars = new Star[3];
            Vector3 starPosition;
            Vector2 rectSize = (_progressBar.transform as RectTransform).sizeDelta;
            Vector2 positionOffset = (_progressBar.transform as RectTransform).anchoredPosition - Vector2.right * rectSize.x / 2f;

            for (int i = 0; i < _stars.Length; i++)
            {
                starPosition = positionOffset + new Vector2(rectSize.x * LevelsManager.progressCheckPoints[i], _starSpawnYOffset);
                _stars[i] = Instantiate(_starPrefab, starPosition, Quaternion.identity);
                _stars[i].transform.SetParent(_progressBar.transform, false);
                _stars[i].Init(i);
            }
        }

        private void InitializedTextLevel()
        {
            int clevel = _levelsData.playerLevel;

            if (clevel < 10)
            {
                _textLevel.text = "LEVEL 0" + clevel;
            }
            else
            {
                _textLevel.text = "LEVEL " + clevel;
            }
        }

        public void ReloadScene()
        {
            GameScenes.ReloadScene();
        }
    }
}
