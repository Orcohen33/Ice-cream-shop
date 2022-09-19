using IceCreamShop.MongoDB.BusinessLogicLayer;
using IceCreamShop.MongoDB.DataAccessLayer.Factory;

namespace IceCreamShop.MongoDB.PresentationLayer
{
    public class MongoDBMain
    {
        public MongoDBMain()
        {
            Console.WriteLine("[main] MongoDB");
            // Create Database if doesnt exists.
            if (!new MongoCreateNFillData().MongoDatabaseExists())
                new MongoCreateNFillData().MongoFillIngredients();
            OrderProcess mongoDBDemo = new();
        }
    }
}
