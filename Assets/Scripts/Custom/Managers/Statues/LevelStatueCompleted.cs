using core;

namespace engine
{
    public class LevelStatueCompleted : GameStatue<ILevelCompleted>
    {
        public override void Start()
        {
            input.ControllerInputs.s_EnableInputs = false;

            Invoke(item => item.LevelCompleted());
        }
    }
}