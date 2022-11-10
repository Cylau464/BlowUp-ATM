using core;
using input;
using engine;
using UnityEngine;
using main.level;
using System;
using apps;

public class GameManager : CoreManager
{
    #region statues
    public static bool isStarted { get; private set; }
    public static bool isCompleted { get; private set; }
    public static bool isFailed { get; private set; }

    public static bool isFinished { get { return isFailed || isCompleted; } }
    public static bool isPlaying { get { return !isFinished && isStarted; } }
    #endregion

    [Header("Levels data")]
    [SerializeField] private LevelsData _levelsData;

    public LevelsData levelsData => _levelsData;

    private IGameStatue _startStatue = new LevelStatueStarted();
    private IGameStatue _failedStatue = new LevelStatueFailed();
    private IGameStatue _completedStatue = new LevelStatueCompleted();

    private float _startTime;

    public static GameManager Instance;

    public static Action OnLevelStarted { get; set; }
    public static Action OnCompleted { get; set; }
    public static Action OnFailed { get; set; }

    private new void Awake()
    {
        base.Awake();

        if(Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    protected override void OnInitialize()
    {

        isStarted = false;
        isCompleted = false;
        isFailed = false;

#if Support_SDK
        apps.ADSManager.DisplayBanner();
#endif
    }

    #region desitions
    public void MakeStarted()
    {
        isStarted = true;
        _startTime = Time.time;

#if Support_SDK
        ProgressStartInfo info = new ProgressStartInfo(
            _levelsData.playerLevel,
            "Level_" + _levelsData.idLevel,
            _levelsData.gameSettings.levelContainer.totalLevels,
            "Basic",
            LevelsManager.currentLevel.levelInfo.LoopLevel,
            _levelsData.randomLevels,
            "Basic",
            "Basic"
            );
        EventsLogger.ProgressStartEvent(info);
#endif

        OnLevelStarted?.Invoke();
        SwitchToStatue(_startStatue);
    }

    public void MakeFailed()
    {
        if (isFinished)
            return;

        isFailed = true;

        ControllerInputs.s_EnableInputs = false;

        _levelsData.OnLost();

#if Support_SDK
        //apps.ProgressEvents.OnLevelFieled(_levelsData.playerLevel);
#endif

        SwitchToStatue(_failedStatue);

        OnFailed?.Invoke();
    }

    public void MakeCompleted()
    {
        if (isFinished)
            return;

        isCompleted = true;

        ControllerInputs.s_EnableInputs = false;

        int playerLevel = _levelsData.playerLevel;
        string time = Mathf.FloorToInt(Time.time - _startTime).ToString();
        _levelsData.OnWin();

#if Support_SDK
        ProgressCompletedInfo info = new ProgressCompletedInfo(
            playerLevel,
            "Level_" + _levelsData.idLevel,
            _levelsData.gameSettings.levelContainer.totalLevels,
            "Basic",
            LevelsManager.currentLevel.levelInfo.LoopLevel,
            _levelsData.randomLevels,
            "Basic",
            "Basic",
            time,
            Mathf.RoundToInt(LevelsManager.currentLevel.LevelProgress * 100),
            0
            );
        EventsLogger.ProgressCompletedEvent(info);
#endif

        SwitchToStatue(_completedStatue);

        OnCompleted?.Invoke();
    }
    #endregion
}