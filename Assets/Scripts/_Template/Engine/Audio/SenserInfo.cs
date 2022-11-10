using core;
using data;
using System;
using UnityEngine;

namespace engine.senser
{
    [CreateAssetMenu(fileName = "New Senser", menuName = "Add/More/New Senser", order = 11)]
    public class SenserInfo : ScriptableObject, IData
    {
        #region delegates
        private event Action<bool> _onSwitch;
        public event Action<bool> onSwitch
        {
            add
            {
                _onSwitch += value;
            }
            remove
            {
                _onSwitch -= value;
            }
        }
        #endregion

        #region variables
        [SerializeField] private Data _data;
        public bool isEnable => _data.isEnable;
        #endregion

        #region engine funs
        public void Awake()
        {
            Initialize();
        }
        #endregion

        #region data
        public void Initialize()
        {
            _data = ES3.Load(GetKey(), ObjectSaver.GetSavingPathFile<Data>(GetKey()), _data);
        }

        public void SaveData()
        {
            ES3.Save(GetKey(), _data, ObjectSaver.GetSavingPathFile<Data>(GetKey()));
        }

        public string GetKey()
        {
            return "Senser." + GetInstanceID();
        }

        [NaughtyAttributes.Button("Reset Data")]
        public void ResetData()
        {
            _data.Reset();
        }
        #endregion

        #region Senser
        /// <summary>
        /// Switch the Senser enable if the Senser was false you can switch it to true and opposite.
        /// </summary>
        public void SwitchEnable()
        {
            _data.isEnable = !_data.isEnable;
            SaveData();
            _onSwitch?.Invoke(_data.isEnable);
        }

        public void SetEnable(bool enable)
        {
            if (enable != _data.isEnable)
            {
                _data.isEnable = enable;
                SaveData();
                _onSwitch?.Invoke(enable);
            }
        }
        #endregion
    }
}
