using core;

namespace engine
{
    public class LevelStatueStarted : GameStatue<ILevelStarted>
    {
        public override void Start()
        {
            input.ControllerInputs.s_EnableInputs = true;

            Invoke(item => item.LevelStarted());
        }
    }
}