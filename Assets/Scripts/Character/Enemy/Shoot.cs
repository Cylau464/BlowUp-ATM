using UnityEngine;

namespace States.Enemy
{
    public class Shoot : State
    {
        private bool _isShooted;
        private CharacterController _target;

        public Shoot(StateMachine machine, EnemyController enemy, CharacterController target) : base(machine, enemy) 
        {
            _target = target;
        }

        public override void Enter()
        {
            base.Enter();

            _enemy.StartShot(_target.transform, Shot);
        }

        public override void Update()
        {
            base.Update();

            if(_isShooted == true && _enemy.CurDelayAfterShot <= 0f)
                _machine.SetState<Patrol>();
        }

        private void Shot()
        {
            _isShooted = true;
        }
    }
}