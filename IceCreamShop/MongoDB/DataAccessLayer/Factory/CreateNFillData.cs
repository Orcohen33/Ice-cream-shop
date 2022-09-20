using IceCreamShop.MongoDB.DataAccessLayer.Imp;
using IceCreamShop.MongoDB.Entity;
using MongoDB.Driver;

namespace IceCreamShop.MongoDB.DataAccessLayer.Factory
{
    public class CreateNFillData
    {
        private readonly IMongoDatabase dbConnection = MongoDBInstance.GetMongoDatabase;
        private string[] dates = new[]
        {
            "2022-09-01",
            "2022-09-07",
            "2022-09-12",
            "2022-09-18",
            "2022-09-23",
            "2022-09-29",
        };

        public CreateNFillData FillIngredients()
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
                    new Ingredient(){name = "Chocolate",          type = "IceCream", price = 6},
                    new Ingredient(){name = "Mekupelet",          type = "IceCream", price = 6},
                    new Ingredient(){name = "Avocado",            type = "IceCream", price = 6},
                    new Ingredient(){name = "Vanilla",            type = "IceCream", price = 6},
                    new Ingredient(){name = "Loacker",            type = "IceCream", price = 6},
                    new Ingredient(){name = "Oreo",               type = "IceCream", price = 6},
                    new Ingredient(){name = "Banana",             type = "IceCream", price = 6},
                    new Ingredient(){name = "Cookie Dough",       type = "IceCream", price = 6},
                    new Ingredient(){name = "Mint Chocolate Chip",type = "IceCream", price = 6},
                    new Ingredient(){name = "Toffee",             type = "IceCream", price = 6},
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
            return this;
        }
        public CreateNFillData MongoCreateSales()
        {
            Random r = new();
            bool chooseChocolate = false, chooseMekupelet = false, chooseVanilla = false;
            Sale sale;
            Ingredient ingredient;
            SaleDAL mongoSaleDAL = new();
            List<Ingredient> Ingredients = (List<Ingredient>)new IngredientDAL().ReadAll();
            #region complete sales
            for (int i = 0; i < 100; i++)
            {
                // initialize sale
                sale = new()
                {
                    order_date = DateTime.TryParse(dates[r.Next(0, dates.Length - 1)], out DateTime date) ? date : DateTime.Now,
                    ingredients = new List<Ingredient>()
                };
                mongoSaleDAL.CreateDocument(sale);
                // choose type of cup
                ingredient = Ingredients.Where(x => x.type.Equals("Box")).ToList()[r.Next(0, 3)];
                sale.ingredients.Add(ingredient);
                // choose balls
                var numOfBalls = ingredient.name.Equals("RegularCup") || ingredient.name.Equals("SpecialCup") ? r.Next(1, 4) : r.Next(1, 7);
                if (numOfBalls == 1) sale.price = 1;

                for (int j = 0; j < numOfBalls; j++)
                {
                    // choose ice cream
                    var iceCreams = Ingredients.Where(x => x.type.Equals("IceCream")).ToList();
                    ingredient = iceCreams[r.Next(0, iceCreams.Count)];
                    sale.ingredients.Add(ingredient);
                    switch (ingredient.name)
                    {
                        case "Chocolate":
                            chooseChocolate = true;
                            break;
                        case "Mekupelet":
                            chooseMekupelet = true;
                            break;
                        case "Vanilla":
                            chooseVanilla = true;
                            break;
                    }
                }
                // choose toppings
                var toppings = Ingredients.Where(x => x.type.Equals("Topping"))
                                          .Where(x => !((chooseChocolate || chooseMekupelet) && x.name == "Chocolate"))
                                          .Where(x => !(chooseVanilla && x.name == "Maple")).ToList();
                var numOfToppings = r.Next(0, 4);
                for (int j = 0; j < numOfToppings; j++)
                {
                    ingredient = toppings[r.Next(0, toppings.Count)];
                    sale.ingredients.Add(ingredient);
                }
                // complete order
                if (sale.price == null) sale.price = 0;
                sale.price += sale.ingredients.Sum(x => x.price);
                mongoSaleDAL.UpdateDocument(sale);
                chooseChocolate = false;
                chooseMekupelet = false;
                chooseVanilla = false;
            }
            #endregion
            #region incomplete sales
            for (int i = 0; i < 30; i++)
            {
                sale = new()
                {
                    order_date = DateTime.Parse(dates[r.Next(0, dates.Length - 1)]),
                    ingredients = new List<Ingredient>()
                };
                mongoSaleDAL.CreateDocument(sale);
            }
            #endregion
            return this;
        }

        public bool MongoDBExists()
        {
            {
                var databases = dbConnection.Client.ListDatabaseNames().ToList();
                return databases.Any(database => database.Equals("IceCreamShop"));
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
