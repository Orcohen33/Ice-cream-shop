using IceCreamShop.MySQL.BusinessLogicLayer;

namespace IceCreamShop.MySQL.PresentationLayer
{
    public class MySQLMain
    {
        public MySQLMain()
        {
            Console.WriteLine("[Main] MySQL");
            //new MySQLCreateNFillData().MySQLcreateTables();
            //new MySQLCreateNFillData().MySQLfillIngredients();
            OrderProcess mySQLDemo = new();
        }
    }
}
