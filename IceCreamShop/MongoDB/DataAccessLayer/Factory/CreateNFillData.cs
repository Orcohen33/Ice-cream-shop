using IceCreamShop.MongoDB.Entity;
using MongoDB.Driver;

namespace IceCreamShop.MongoDB.DataAccessLayer.Factory
{
    public class MongoCreateNFillData
    {
        private readonly IMongoDatabase dbConnection = MongoDBInstance.GetMongoDatabase;


        public void MongoFillIngredients()
        {
            try
            {
                if (!MongoCollectionsExists())
                {
                    dbConnection.CreateCollection("Sales");
                    dbConnection.CreateCollection("Ingredients");
                }
                var collection = dbConnection.GetCollection<Ingredient>("Ingredients");


                /* You can add/delete ingredients whenever you want */
                var ingredients = new List<Ingredient>()
                {
                    new Ingredient(){name = "Chocolate",  type = "IceCream", price = 6},
                    new Ingredient(){name = "Vanilla",    type = "IceCream", price = 6},
                    new Ingredient(){name = "Strawberry", type = "IceCream", price = 6},
                    new Ingredient(){name = "Mekupelet",  type = "IceCream", price = 6},
                    new Ingredient(){name = "Mint",       type = "IceCream", price = 6},
                    new Ingredient(){name = "Caramel",    type = "IceCream", price = 6},
                    new Ingredient(){name = "Chocolate",  type = "Topping", price = 2},
                    new Ingredient(){name = "Maple",      type = "Topping", price = 2},
                    new Ingredient(){name = "Caramel",    type = "Topping", price = 2},
                    new Ingredient(){name = "Strawberry", type = "Topping", price = 2},
                    new Ingredient(){name = "Peanut",     type = "Topping", price = 2},
                    new Ingredient(){name = "RegularCup", type = "Box", price = 0},
                    new Ingredient(){name = "SpecialCup", type = "Box", price = 2},
                    new Ingredient(){name = "Box",        type = "Box", price = 5}
                };
                collection.InsertMany(ingredients);
            }
            catch (Exception err)
            {
                Console.WriteLine($"{err.Message}");
            }
        }
        public void MongoCreateSales()
        {
            // generate 10 sales
            var sales = new List<Sale>();


        }

        public bool MongoDatabaseExists()
        {
            {
                var databases = dbConnection.Client.ListDatabaseNames().ToList();
                return databases.Any(database => database.Equals("IceCreamStore"));
            }
        }
        private bool MongoCollectionsExists()
        {
            // Check if 'Ingredients' and 'Sales' collections exists
            var collectionsList = dbConnection.ListCollections().ToList();
            var ingredientExist = collectionsList.Any(database => database.Equals("Ingredients"));
            var salesExist = collectionsList.Any(database => database.Equals("Sales"));
            return ingredientExist && salesExist;
        }

    }

}
