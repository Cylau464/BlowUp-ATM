using System;
using UnityEngine;
using States.Enemy;

namespace States
{
    public class StateMachine
    {
        private State _state;
        private EnemyController _enemy;

        private Action OnStartBlow;
        public Action OnStateSwitched;

        public StateMachine(EnemyController enemy)
        {
            _enemy = enemy;
            OnStartBlow = SetState<Blow>;
            _enemy.OnStartBlow += OnStartBlow;
        }

        ~StateMachine()
        {
            if(_enemy != null)
                _enemy.OnStartBlow -= OnStartBlow;
        }

        public void SetState<T>() where T : State
        {
            if (_state != null)
                _state.Exit();

            _state = (T)Activator.CreateInstance(typeof(T), this, _enemy);
            _state.Enter();
            OnStateSwitched?.Invoke();
        }

        public void SetState<T>(CharacterController target) where T : State
        {
            if (_state != null)
                _state.Exit();

            _state = (T)Activator.CreateInstance(typeof(T), this, _enemy, target);
            _state.Enter();
            OnStateSwitched?.Invoke();
        }

        public void Update()
        {
            if(_state != null)
                _state.Update();
        }
    }
}
