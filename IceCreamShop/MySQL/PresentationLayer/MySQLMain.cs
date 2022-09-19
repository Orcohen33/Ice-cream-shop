using IceCreamShop.MySQL.BusinessLogicLayer;
using IceCreamShop.MySQL.DataAccessLayer.Factory;

namespace IceCreamShop.MySQL.PresentationLayer
{
    public class MySQLMain
    {
        public MySQLMain()
        {
            Console.WriteLine("[Main] MySQL");

            if (!new CreateNFillData().MySQLExists())
            {
                new CreateNFillData()
                    .createTables()
                    .FillIngredients();
            }
            OrderProcess mySQLDemo = new();
        }
    }
}
