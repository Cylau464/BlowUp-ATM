using events;
using System;

namespace core
{
    public abstract class GameStatue<T> : IGameStatue
    {
        private static GameEvent<T> _container = new GameEvent<T>();

        public virtual void End() { }

        public virtual void Start() { }

        protected static void Invoke(Action<T> action)
        {
            _container.Invoke(action);
        }

        public static void Subscribe(T interfaceSubscribe)
        {
            _container.Subscribe(interfaceSubscribe);
        }

        public static void Unsubscribe(T interfaceSubscribe)
        {
            _container.Unsubscribe(interfaceSubscribe);
        }
    }
}
