using core;

namespace engine
{
    public class LevelStatueFailed : GameStatue<ILevelFailed>
    {
        public override void Start()
        {
            input.ControllerInputs.s_EnableInputs = false;

            Invoke(item => item.LevelFailed());
        }
    }
}