using UnityEngine;

namespace engine.senser
{
    [System.Serializable]
    public struct Data
    {
        [Tooltip("In true case the on senser.")]
        public bool isEnable;
        
        /// <summary>
        /// Restore the data to the default values.
        /// </summary>
        public void Reset()
        {
            isEnable = true;
        }
    }
}
