using core;
using data;
using UnityEngine;

namespace main.level
{
    [CreateAssetMenu(fileName = "New LevelsData", menuName = "Add/More/LevelsData", order = 7)]
    public class LevelsData : ScriptableObject, IData
    {
        [SerializeField] private Data _data;
        [SerializeField] private GameSettings _gameSettings;

        #region gets sets
        public GameSettings gameSettings => _gameSettings;
        public int playerLevel => _data.playerLevel;
        public int idLevel => GetIDLevel();
        public bool randomLevels => _data.randomLevels;

        public int GetIDLevel()
        {
            /// If the game in testing mode.
            if (_gameSettings.isTestingMode)
                return _gameSettings.indexTestLevel;
            else
                return _data.idLevel;
        }
        #endregion

        #region inits
        public void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            _data = ES3.Load(GetKey(), ObjectSaver.GetSavingPathFile<Data>(GetKey()), _data);
        }
        #endregion

        #region progress
        /// <summary>
        /// This function execute when the anylevel completed .
        /// </summary>
        public void OnWin()
        {
            if (_data.randomLevels)
            {
                _data.idLevel = _gameSettings.GetRandomLevelID(_data.idLevel);
            }
            else
            {
                if (_gameSettings.totalLevels - 1 <= _data.idLevel)
                {
                    _data.randomLevels = true;
                    _data.idLevel = _gameSettings.GetRandomLevelID(_data.idLevel);
                }
                else
                {
                    _data.idLevel++;
                }
            }

            _data.playerLevel++;
            SaveData();
        }

        /// <summary>
        /// This function call on the anylevel failed.
        /// </summary>
        public void OnLost()
        {
            
        }

        /// <summary>
        /// Back to previous level
        /// </summary>
        public void PreviousLevel()
        {
            if (_data.randomLevels)
            {
                _data.idLevel = _gameSettings.totalLevels - 1;
                _data.randomLevels = false;
                _data.playerLevel--;
            }
            else
            {
                if (_data.idLevel > 0)
                {
                    _data.idLevel--;
                    _data.playerLevel--;
                }
                else
                {
                    _data.idLevel = 0;
                    _data.playerLevel = 1;
                }
            }

            SaveData();
        }
        
        #endregion

        #region data management
        public void SaveData()
        {
            ES3.Save(GetKey(), _data, ObjectSaver.GetSavingPathFile<Data>(GetKey()));
        }

        public string GetKey()
        {
            return "LevelsData." + GetInstanceID();
        }

        [NaughtyAttributes.Button("Reset Data")]
        public void ResetData()
        {
            _data.Reset();
        }
        #endregion
    }
}