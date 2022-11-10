namespace core
{
    public interface IData : IResetData, IAwake
    {
        /// <summary>
        /// Get the key of saving data.
        /// </summary>
        string GetKey();

        void SaveData();
    }
}