using UnityEngine;

namespace main.level
{
    [CreateAssetMenu(fileName = "Loader Assets", menuName = "Add/More/Loader Assets", order = 10)]
    public class LevelContainer : ScriptableObject
    {
        #region vars
        [Header("Data")]
        [SerializeField] protected LevelsData _levelsData;

        [Header("Levels")]
        [SerializeField] protected Level[] _levels;
        #endregion

        #region gets
        public int totalLevels => _levels.Length;
        public Level currentLevel => GetLevel(_levelsData.GetIDLevel());
        public Level GetLevel(int idLevel)
        {
            if ((uint)_levels.Length <= (uint)idLevel) throw new System.ArgumentOutOfRangeException();
            return _levels[idLevel];
        }
        #endregion
    }
}
