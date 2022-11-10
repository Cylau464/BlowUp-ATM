namespace States
{
    public class State
    {
        protected StateMachine _machine;
        protected EnemyController _enemy;

        public State(StateMachine machine, EnemyController enemy)
        {
            _machine = machine;
            _enemy = enemy;
        }

        public virtual void Enter() { }
        public virtual void Update() { }
        public virtual void Exit() { }
    }
}
