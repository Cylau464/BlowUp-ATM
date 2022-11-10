using UnityEngine;

namespace States.Enemy
{
    public class Chase : State
    {
        private CharacterController _target;

        public Chase(StateMachine machine, EnemyController enemy) : base(machine, enemy) { }

        public override void Enter()
        {
            base.Enter();
            _target = _enemy.GetTarget().GetComponent<CharacterController>();
            _target.OnShocked += CancelChase;
        }

        public override void Update()
        {
            base.Update();

            switch(_enemy.ChaseTarget(_target.transform))
            {
                case ChaseStatus.Abort:
                    _machine.SetState<Patrol>();
                    break;
                case ChaseStatus.CuaghtUp:
                    _machine.SetState<Aim>(_target);
                    break;
                case ChaseStatus.Chase:
                    break;
            }
        }

        public override void Exit()
        {
            base.Exit();

            _enemy.SetChaseDelay();
            _target.OnShocked -= CancelChase;
        }

        private void CancelChase()
        {
            _machine.SetState<Patrol>();
        }
    }

    public enum ChaseStatus { Chase, Abort, CuaghtUp }
}
