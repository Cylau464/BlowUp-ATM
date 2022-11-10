namespace engine.store
{
    public interface IStore
    {
        bool DeselectProduct();
        bool AllowSelect(int idProduct);
        bool SelectProduct(int idProduct);

        bool AllowBuy(int idProduct);
        bool BuyProduct(int idProduct);

        int GetTotalProducts();
        int GetIDSelectedProduct();
        IProduct GetProduct(int idProduct);
        ProductStatue GetProductState(int idProduct);
    }
}
