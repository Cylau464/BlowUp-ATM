namespace core
{
    public abstract class CoreManager : StatueManager
    {
        #region initialize
        protected void Awake()
        {
            TimeManager.Initialize();

            OnInitialize();
        }

        protected abstract void OnInitialize();
        #endregion
    }
}