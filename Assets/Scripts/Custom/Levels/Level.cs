using core;
using engine.coin;
using System;
using UnityEngine;

namespace main.level
{
    public class Level : MonoBehaviour, IValidate
    {
        [SerializeField] protected LevelInfoSO m_LevelInfo;
        [SerializeField] private ATM[] _atms;
        [SerializeField] private CoinsData _coinsData;

        public float LevelProgress { get { return (float)CurrentMoney / MaxMoney; } }
        public int CurrentMoney { get; private set; }
        public int MinMoney { get; private set; }
        public int MaxMoney { get; private set; }
        public int Stars;

        public LevelInfoSO levelInfo => m_LevelInfo;
        public LevelsManager levelsManager { get; private set; }
        public SceneContainer sceneContainer { get; private set; }

        public Action<int> OnCollectMoney { get; set; }

        public virtual void Initialize(LevelsManager levelsManager, SceneContainer sceneContainer)
        {
            this.levelsManager = levelsManager;
            this.sceneContainer = sceneContainer;

            int money = 0;

            foreach (ATM atm in _atms)
                money += atm.MoneyCount;

            MinMoney = Mathf.FloorToInt(money * LevelsManager.progressCheckPoints[0]);
            MaxMoney = money;
        }

        public void Validate()
        {
#if UNITY_EDITOR
            _atms = editor.EditorManager.FindScenesComponents<ATM>();
#endif
        }

        public void CollectMoney(int count)
        {
            CurrentMoney += count;
            _coinsData.AddCoins(count);

            OnCollectMoney?.Invoke(CurrentMoney);
        }
    }
}
