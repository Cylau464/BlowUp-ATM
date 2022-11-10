using UnityEngine;

namespace States.Enemy
{
    public class Aim : State
    {
        private CharacterController _target;

        public Aim(StateMachine machine, EnemyController enemy, CharacterController target) : base(machine, enemy)
        {
            _target = target;
            _target.OnShocked += CancelAim;
        }

        public override void Enter()
        {
            base.Enter();

            _enemy.StartAim();
        }

        public override void Update()
        {
            base.Update();

            if(_enemy.Aiming(_target.transform) == true)
                _machine.SetState<Shoot>(_target);
        }

        public override void Exit()
        {
            base.Exit();

            _target.OnShocked -= CancelAim;
        }

        private void CancelAim()
        {
            _machine.SetState<Patrol>();
        }
    }
}