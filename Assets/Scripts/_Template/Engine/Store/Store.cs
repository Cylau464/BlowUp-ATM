using System;
using data;
using core;
using UnityEngine;
using engine.coin;

namespace engine.store
{
    [CreateAssetMenu(fileName = "New Store", menuName = "Add/Store/Add Store", order = 1)]
    public class Store : ScriptableObject, IStore, IData
    {
        #region delegates
        protected event Action<IProduct, ProductStatue> _handleRefresh;
        public event Action<IProduct, ProductStatue> handleRefresh
        {
            add
            {
                _handleRefresh += value;
            }
            remove
            {
                _handleRefresh -= value;
            }
        }
        #endregion

        #region variables
        [Header("Data")]
        [SerializeField] private StoreData _data;

        [Header("Settings")]
        [SerializeField] private Product[] _products;
        #endregion

        #region inits
        public void Awake()
        {
            LoadData();

            InitializeProducts();
        }

        private void InitializeProducts()
        {
            if (_products == null)
                return;

            for (int i = 0; i < _products.Length; i++)
            {
                _products[i].Initialize(this, i);
            }
        }

        public void RefreshProducts()
        {
            for (int i = 0; i < _products.Length; i++)
            {
                _products[i].UpdateState();
            }
        }
        #endregion

        #region data
        public void LoadData()
        {
            _data = ES3.Load(GetKey(), ObjectSaver.GetSavingPathFile<Data>(GetKey()), _data);
        }

        public void SaveData()
        {
            ES3.Save(GetKey(), _data, ObjectSaver.GetSavingPathFile<Data>(GetKey()));
        }

        public void ResetData()
        {
            if (_products != null && _products.Length != 0)
            {
                _data = new StoreData();
                _data.idSelectedProduct = 0;
                _data.isBoughtProducts = new bool[_products.Length];

                if (_data.isBoughtProducts.Length != 0) _data.isBoughtProducts[0] = true;
            }
        }

        public string GetKey()
        {
            return "Store." + GetInstanceID();
        }

        protected void OnValidate()
        {
            if (_data.isBoughtProducts.Length != _products.Length)
                ResetData();
        }
        #endregion

        #region select
        public bool DeselectProduct()
        {
            if (_data.idSelectedProduct < 0)
                return false;

            _products[_data.idSelectedProduct].Deselect();
            _handleRefresh?.Invoke(_products[_data.idSelectedProduct], ProductStatue.Bought);
            _data.idSelectedProduct = -1;
            return true;
        }

        /// <summary>
        /// If user the use can select or choice this product.
        /// </summary>
        /// <param name="idProduct"> The id of the product. </param>
        /// <returns> True if product is enable for select.</returns>
        public virtual bool AllowSelect(int idProduct)
        {
            if (idProduct < 0 || _products.Length <= idProduct)
            {
                Debug.LogError("The id is out of array lenght: ID " + idProduct + ", Array Lenght: " + _products.Length);
                return false;
            }

            return _data.isBoughtProducts[idProduct] && _data.idSelectedProduct != idProduct && _products[idProduct].AllowSelect();
        }

        public bool SelectProduct(int idProduct)
        {
            if (!AllowSelect(idProduct))
                return false;
            else
            {
                // Deselect the old id.
                DeselectProduct();

                // update data product.
                _data.idSelectedProduct = idProduct;

                // Execut select on the product class.
                _products[idProduct].Selected();
                _handleRefresh?.Invoke(_products[idProduct], ProductStatue.Selected);

                // Save data.
                SaveData();
                return true;
            }
        }
        #endregion

        #region buy
        /// <summary>
        /// If user the use can Buy this product.
        /// </summary>
        /// <param name="idProduct"> The id of the product. </param>
        /// <returns> True if product is enable for buy.</returns>
        public virtual bool AllowBuy(int idProduct)
        {
            if (idProduct < 0 || _products.Length <= idProduct)
            {
                Debug.LogError("The id is out of array lenght: ID " + idProduct + ", Array Lenght: " + _products.Length);
                return true;
            }

            return !_data.isBoughtProducts[idProduct] && _products[idProduct].AllowBuy();
        }

        public bool BuyProduct(int idProduct)
        {
            if (!AllowBuy(idProduct))
                return false;

            DeselectProduct();

            // Update data
            _data.idSelectedProduct = idProduct;
            _data.isBoughtProducts[idProduct] = true;

            // Execut buy on the product class.
            _products[idProduct].Buy();
            _handleRefresh?.Invoke(_products[idProduct], ProductStatue.Selected);

            // Save data.
            SaveData();
            return true;
        }
        #endregion

        #region info
        public IProduct GetProduct(int idProduct)
        {
            if (idProduct < 0 || _products.Length <= idProduct)
            {
                Debug.LogError("The id is out of array lenght: ID " + idProduct + ", Array Lenght: " + _products.Length);
                return null;
            }

            return _products[idProduct];
        }

        public int GetTotalProducts()
        {
            return _products.Length;
        }

        public int GetIDSelectedProduct()
        {
            return _data.idSelectedProduct;
        }

        public virtual ProductStatue GetProductState(int idProduct)
        {
            if ((uint)_data.isBoughtProducts.Length <= (uint)idProduct) throw new ArgumentOutOfRangeException();

            if (GetIDSelectedProduct() == idProduct)
                return ProductStatue.Selected;
            else
            if (_data.isBoughtProducts[idProduct])
                return ProductStatue.Bought;
            else
                return ProductStatue.ForBuy;
        }
        #endregion
    }
}
