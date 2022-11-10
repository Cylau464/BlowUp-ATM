using main.level;

public class StarProgress : Star
{
    private void OnEnable()
    {
        LevelsManager.currentLevel.OnCollectMoney += CheckProgress;
    }

    private void OnDisable()
    {
        if(LevelsManager.currentLevel != null)
            LevelsManager.currentLevel.OnCollectMoney -= CheckProgress;
    }

    public override void Fill()
    {
        base.Fill();

        LevelsManager.currentLevel.Stars++;
        LevelsManager.currentLevel.OnCollectMoney -= CheckProgress;
    }

    private void CheckProgress(int count)
    {
        if (LevelsManager.currentLevel.LevelProgress >= LevelsManager.progressCheckPoints[_index])
            Fill();
    }
}