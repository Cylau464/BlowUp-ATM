using core;
using data;
using System;
using UnityEngine;

namespace engine.coin
{
    public enum OperationType { Add, Minus }

    public struct ParametersUpdate
    {
        public int total;
        public int amount;
        public OperationType operation;
    }

    [Serializable]
    public struct Data
    {
        [Tooltip("Total saving coins in the game.")]
        public int totalCoins;
    }

    [CreateAssetMenu(fileName = "New Coins Data", menuName = "Add/Coins Data", order = 3)]
    public class CoinsData : ScriptableObject, IData
    {
        #region delegate
        private event Action<ParametersUpdate> _onUpdate;
        /// <summary>
        /// On add or minus coins. We will call this delegate.
        /// First parameter content the total and second content the amount adding.
        /// </summary>
        public event Action<ParametersUpdate> onUpdate
        {
            add
            {
                _onUpdate += value;
            }
            remove
            {
                _onUpdate -= value;
            }
        }
        #endregion

        #region variables
        [Header("Data")]
        [Tooltip("Currect data saving values.")]
        [SerializeField] private Data _data;

        [Header("Settings")]
        [Tooltip("Initialize total coins on start the game first time.")]
        public int initCoins = 0;

        #region gets
        public int totalCoins => _data.totalCoins;
        #endregion
        #endregion

        #region engine
        public void Awake()
        {
            Initialize();
        }

        public string GetKey()
        {
            return "Coins." + GetInstanceID();
        }

        public void Initialize()
        {
            _data = ES3.Load(GetKey(), ObjectSaver.GetSavingPathFile<Data>(GetKey()), _data);
        }

        public void SaveData()
        {
            ES3.Save(GetKey(), _data, ObjectSaver.GetSavingPathFile<Data>(GetKey()));
        }

        [NaughtyAttributes.Button("Reset Data")]
        public void ResetData()
        {
            _data.totalCoins = initCoins;
        }
        #endregion

        #region operations
        public virtual bool AddCoins(int amount)
        {
            if (amount <= 0)
                return false;

            /// Update data.
            _data.totalCoins += amount;

            /// Fill data delegate.
            ParametersUpdate dData = new ParametersUpdate();
            dData.total = _data.totalCoins;
            dData.amount = amount;
            dData.operation = OperationType.Add;

            SaveData();
            // Execute delegate.
            _onUpdate?.Invoke(dData);
            return true;
        }

        public virtual bool RemoveCoins(int amount)
        {
            if (amount <= 0 || _data.totalCoins < amount)
                return false;

            /// Update data.
            _data.totalCoins = Mathf.Clamp(_data.totalCoins - amount, 0, _data.totalCoins);

            /// Fill data delegate.
            ParametersUpdate dData = new ParametersUpdate();
            dData.total = _data.totalCoins;
            dData.amount = amount;
            dData.operation = OperationType.Minus;

            SaveData();
            // Execute delegate.
            _onUpdate?.Invoke(dData);
            return true;
        }
        #endregion
    }
}