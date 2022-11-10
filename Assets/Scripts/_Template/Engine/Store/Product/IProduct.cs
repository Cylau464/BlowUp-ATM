namespace engine.store
{
    public interface IProduct
    {
        void Initialize(IStore store, int id);

        bool AllowBuy();
        bool Buy();

        bool Selected();
        bool AllowSelect();
        bool Deselect();

        bool UpdateState();
    }
}
