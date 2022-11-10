using engine;
using engine.camera;
using UnityEngine;

namespace examples
{
    [System.Serializable]
    public class LevelsLogic : ILevelFailed, ILevelStarted
    {
        public VirtualCamerasManager virtualCamerasManager;

        public string _startViewTag = "OnStart";
        public CameraView _loseView;

        public void LevelFailed()
        {
            virtualCamerasManager.AddCameraViewAndSwitch(_loseView);
        }

        public void LevelStarted()
        {
            virtualCamerasManager.SwitchTo(_startViewTag);
        }
    }

    public class SwitchCamerasExample : MonoBehaviour
    {
        [SerializeField] private LevelsLogic _controllerViewCamera;

        void OnEnable()
        {
            LevelStatueStarted.Subscribe(_controllerViewCamera);
            LevelStatueFailed.Subscribe(_controllerViewCamera);
        }
    }
}
