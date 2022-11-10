namespace events
{
    public interface IEvent<T>
    {
        void Subscribe(T handler);

        void Unsubscribe(T handler);
    }
}