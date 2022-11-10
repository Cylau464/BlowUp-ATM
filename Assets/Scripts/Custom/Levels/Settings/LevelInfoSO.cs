using core;
using UnityEngine;

namespace main.level
{
    [CreateAssetMenu(fileName = "LevelInfo", menuName = "Add/LevelInfo", order = 1)]
    public class LevelInfoSO : ScriptableObject, IResetData
    {
        public int LoopLevel;

        public void ResetData()
        {
            LoopLevel = 0;
        }
    }
}
