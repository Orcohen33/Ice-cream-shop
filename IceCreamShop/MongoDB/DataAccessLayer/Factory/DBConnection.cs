using MongoDB.Driver;

namespace IceCreamShop.MongoDB.DataAccessLayer.Factory
{
    //Mark the class as 'sealed' so as not to make this class inheritable. Optional.
    public sealed class MongoDBInstance
    {
        //volatile: ensure that assignment to the instance variable
        //is completed before the instance variable can be accessed
        private static volatile MongoDBInstance instance;
        private static object syncLock = new object();


        MongoClientSettings settings = MongoClientSettings.FromConnectionString("mongodb://localhost:27017"); // Change the connection string to your own MongoDB connection string
        private static IMongoDatabase? db = null;

        private MongoDBInstance()
        {
            try
            {
                var client = new MongoClient(settings);
                db = client.GetDatabase("IceCreamStore");
                Console.WriteLine("[MongoDBInstance] Connecting to MongoDB...");
            }
            catch (Exception err)
            {
                Console.WriteLine($"{err.Message}");
            }
        }


        public static IMongoDatabase GetMongoDatabase
        {
            get
            {
                if (instance == null)
                {
                    lock (syncLock)
                    {
                        if (instance == null)
                            instance = new MongoDBInstance();
                    }
                }

                return db;
            }
        }
    }
}
