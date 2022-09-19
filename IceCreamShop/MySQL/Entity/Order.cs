namespace IceCreamShop.MySQL.Entity
{
    class Order
    {
        private int _SaleId;
        private int _IngredientId;

        public int SaleId { get => _SaleId; set => _SaleId = SaleId; }
        public int IngredientId { get => _IngredientId; set => _IngredientId = IngredientId; }
        public Order(int sid, int iid)
        {
            _SaleId = sid;
            _IngredientId = iid;
        }
    }
}
