using engine.random;
using main.level;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Settings", menuName = "Add/More/Game Settings", order = 1)]
public class GameSettings : ScriptableObject
{
    [Header("Scenes names")]
    [SerializeField] private string _mainSceneName = "Main";
    [Header("Mode Test")]
    public bool isTestingMode = false;
    public int indexTestLevel = 0;

    public int totalLevels => _levelContainers[0].totalLevels;
    public int startRandomLevel = 0;

    [Header("Level Containers")]
    [SerializeField] private LevelContainer[] _levelContainers;

    public LevelContainer levelContainer => _levelContainers[0];

    #region gets
    public string mainSceneName => _mainSceneName;

    /// <summary>
    /// Get random level with not repeated the last id level.
    /// </summary>
    public int GetRandomLevelID(int lastIdLevel)
    {
        if (1 < totalLevels)
        {
            RandomFieldInfo[] fieldInfos;

            if (lastIdLevel <= startRandomLevel)
            {
                fieldInfos = new RandomFieldInfo[1];
                fieldInfos[0] = new RandomFieldInfo(startRandomLevel + 1, totalLevels);
            }
            else
            if (totalLevels - 1 <= lastIdLevel)
            {
                fieldInfos = new RandomFieldInfo[1];
                fieldInfos[0] = new RandomFieldInfo(startRandomLevel, totalLevels - 1);
            }
            else
            {
                fieldInfos = new RandomFieldInfo[2];
                fieldInfos[0] = new RandomFieldInfo(startRandomLevel, lastIdLevel - 1);
                fieldInfos[1] = new RandomFieldInfo(lastIdLevel + 1, totalLevels);
            }

            return IntelligentRandom.GetRandomWithField(fieldInfos);
        }
        else
            return 0;
    }

    protected void OnValidate()
    {
        indexTestLevel = Mathf.Clamp(indexTestLevel, 0, totalLevels);
    }
    #endregion
}
