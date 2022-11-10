using UnityEngine;

namespace States.Enemy
{
    public class Patrol : State
    {
        public Patrol(StateMachine machine, EnemyController enemy) : base(machine, enemy) { }

        public override void Enter()
        {
            _enemy.MoveToNextPoint();
        }

        public override void Update()
        {
            if (_enemy.CheckTarget() == true)
            {
                _machine.SetState<Chase>();

                return;
            }

            if (_enemy.IsDestinationReached() == true)
                _enemy.Wait();
        }
    }
}
