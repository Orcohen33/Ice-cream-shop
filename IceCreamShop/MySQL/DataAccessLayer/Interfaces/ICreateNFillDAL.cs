namespace IceCreamShop.MySQL.DataAccessLayer.Interfaces
{
    internal interface ICreateNFillDAL
    {
        void MySQLcreateTables();
        void MySQLfillIngredients();
    }
}
