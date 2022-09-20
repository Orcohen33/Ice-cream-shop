using IceCreamShop.MySQL.DataAccessLayer.Imp;
using IceCreamShop.MySQL.Entity;
using MySql.Data.MySqlClient;
#pragma warning disable
namespace IceCreamShop.MySQL.DataAccessLayer.Factory
{
    class CreateNFillData
    {
        #region Variables
        private readonly DbContext dbContext = DbContext.Instance;
        private string[] dates = new[]
        {
            "2022-09-01",
            "2022-09-07",
            "2022-09-12",
            "2022-09-18",
            "2022-09-23",
            "2022-09-29",

        };

        internal bool MySQLExists()
        {
            try
            {
                dbContext.conn.Open();
                var sql = "SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = 'icecreamshop'";
                var cmd = new MySqlCommand(sql, dbContext.conn);
                var reader = cmd.ExecuteReader();
                var exists = reader.HasRows;
                reader.Close();
                Console.WriteLine("[MySQLExists] MySQL exists: " + exists);
                return exists;
            }
            catch (Exception err)
            {
                Console.WriteLine($"{err.Message}");
            }
            finally
            {
                dbContext.conn.Close();
            }
            return false;
        }

        #endregion
        public CreateNFillData createTables()
        {
            try
            {
                dbContext.conn.Open();

                string sql = "DROP DATABASE IF EXISTS icecreamshop;";
                MySqlCommand cmd = new MySqlCommand(sql, dbContext.conn);
                var rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0 || rowsAffected == -1)
                {
                    Console.WriteLine("[CreateNFillData] Database drop failed");
                }
                else
                {
                    Console.WriteLine("[CreateNFillData] Database dropped successfully");
                }

                // create icecreamshop schema
                sql = "CREATE DATABASE icecreamshop;";
                cmd = new MySqlCommand(sql, dbContext.conn);
                cmd.ExecuteNonQuery();

                // create Sales
                sql = "CREATE TABLE `icecreamshop`.`Sales` (" +
                    "`sid` INT NOT NULL AUTO_INCREMENT, " +
                    "`order_date` DATE NOT NULL," +
                    "`price` INT NULL," +
                    "PRIMARY KEY (`sid`));";

                cmd = new MySqlCommand(sql, dbContext.conn);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Created Sales");

                // create Ingredient
                sql = "CREATE TABLE `icecreamshop`.`ingredients` (" +
                    "`iid` INT NOT NULL AUTO_INCREMENT, " +
                    "`name` VARCHAR(45) NOT NULL," +
                    "`type` VARCHAR(45) NOT NULL," +
                    "`price` INT NOT NULL," +
                    "PRIMARY KEY (`iid`));";

                cmd = new MySqlCommand(sql, dbContext.conn);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Created ingredients");

                // create orders
                sql = "CREATE TABLE `icecreamshop`.`Orders` (" +
                    "`id` INT NOT NULL AUTO_INCREMENT PRIMARY KEY, " +
                    "`sid` INT NOT NULL," +
                    "`iid` INT NOT NULL," +
                 "CONSTRAINT fk_OrdersSid FOREIGN KEY (sid) " +
                "REFERENCES icecreamshop.Sales(sid), " +
                "CONSTRAINT fk_OrdersIid FOREIGN KEY(iid) " +
                "REFERENCES icecreamshop.ingredients(iid));";
                cmd = new MySqlCommand(sql, dbContext.conn);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Created orders");

                dbContext.conn.Close();
            }
            catch (Exception ex) when (ex is MySqlException)
            {
                Console.WriteLine(ex.Message);
            }

            return this;
        }
        public CreateNFillData FillIngredients()
        {
            try
            {
                dbContext.conn.Open();
                string sql = "INSERT INTO icecreamshop.ingredients(`name`, `type`,`price`) VALUES(\"Chocolate\", \"IceCream\", 6);" +
                "INSERT INTO icecreamshop.ingredients(`name`, `type`,`price`) VALUES(\"Mekupelet\", \"IceCream\", 6);" +
                "INSERT INTO icecreamshop.ingredients(`name`, `type`,`price`) VALUES(\"Avocado\", \"IceCream\", 6);" +
                "INSERT INTO icecreamshop.ingredients(`name`, `type`,`price`) VALUES(\"Vanilla\", \"IceCream\", 6);" +
                "INSERT INTO icecreamshop.ingredients(`name`, `type`,`price`) VALUES(\"Loacker\", \"IceCream\", 6);" +
                "INSERT INTO icecreamshop.ingredients(`name`, `type`,`price`) VALUES(\"Oreo\", \"IceCream\", 6);" +
                "INSERT INTO icecreamshop.ingredients(`name`, `type`,`price`) VALUES(\"Banana \", \"IceCream\", 6);" +
                "INSERT INTO icecreamshop.ingredients(`name`, `type`,`price`) VALUES(\"Cookie Dough\", \"IceCream\", 6);" +
                "INSERT INTO icecreamshop.ingredients(`name`, `type`,`price`) VALUES(\"Mint Chocolate Chip\", \"IceCream\", 6);" +
                "INSERT INTO icecreamshop.ingredients(`name`, `type`,`price`) VALUES(\"Toffee\", \"IceCream\", 6);" +

                "INSERT INTO icecreamshop.ingredients(`name`,`type`,`price`) VALUES(\"RegularCup\", \"Box\", 0);" +
                "INSERT INTO icecreamshop.ingredients(`name`,`type`,`price`) VALUES(\"SpecialCup\", \"Box\", 2);" +
                "INSERT INTO icecreamshop.ingredients(`name`,`type`,`price`) VALUES(\"Box\", \"Box\", 5);" +

                "INSERT INTO icecreamshop.ingredients(`name`,`type`,`price`) VALUES(\"Chocolate\", \"Topping\", 2);" +
                "INSERT INTO icecreamshop.ingredients(`name`,`type`,`price`) VALUES(\"Maple\", \"Topping\", 2);" +
                "INSERT INTO icecreamshop.ingredients(`name`,`type`,`price`) VALUES(\"Oreos\", \"Topping\", 2);" +
                "INSERT INTO icecreamshop.ingredients(`name`,`type`,`price`) VALUES(\"Caramel\", \"Topping\", 2);" +
                "INSERT INTO icecreamshop.ingredients(`name`,`type`,`price`) VALUES(\"Peanut\", \"Topping\", 2);";

                MySqlCommand cmd = new MySqlCommand(sql, dbContext.conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception err)
            {
                Console.WriteLine($"[fillIngredients] Error: {err.Message}");
            }
            finally
            {
                dbContext.conn.Close();
            }
            return this;
        }

        public CreateNFillData fillIncompleteAndCompleteSales()
        {
            bool chooseChocolate = false, chooseMekupelet = false, chooseVanilla = false;
            Random r = new();
            int randDates = r.Next(0, dates.Length);
            Sale sale;
            Ingredient ingredient;
            SaleDAL saleDAL = new();
            IngredientDAL ingredientDAL = new();
            OrderDAL orderDAL = new();
            try
            {
                #region complete sales
                for (int i = 0; i < 100; i++)
                {
                    // initialize sale
                    sale = new(dates[r.Next(0, dates.Length - 1)]);
                    saleDAL.createRecord(sale);
                    sale.setId(saleDAL.GetPK());
                    // choose type of cup
                    ingredient = ingredientDAL.readAll()
                                                    .Where(x => x.Type == "Box")
                                                    .ToList()[r.Next(0, 3)];
                    sale.orders.Add(new(sale.Id, ingredient.Id));
                    // choose balls
                    var numOfBalls = ingredient.Name.Equals("RegularCup") || ingredient.Name.Equals("SpecialCup") ? r.Next(1, 4) : r.Next(1, 7);
                    if (numOfBalls == 1) sale.setPrice(1);

                    for (int j = 0; j < numOfBalls; j++)
                    {
                        ingredient = ingredientDAL.readAll()
                                                    .Where(x => x.Type == "IceCream")
                                                    .ToList()[r.Next(0, 9)];
                        sale.orders.Add(new(sale.Id, ingredient.Id));
                        switch (ingredient.Name)
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
                    var numOfToppings = r.Next(0, 4);
                    var kindOfToppings = ingredientDAL.readAll().Where(x => x.Type == "Topping")
                                                                .Where(x => !((chooseChocolate || chooseMekupelet) && x.Name == "Chocolate"))
                                                                .Where(x => !(chooseVanilla && x.Name == "Maple")).ToList();
                    for (int j = 0; j < numOfToppings; j++)
                    {
                        sale.orders.Add(new(sale.Id, kindOfToppings[r.Next(0, kindOfToppings.Count - 1)].Id));
                    }
                    // complete order
                    foreach (Order order in sale.orders)
                        orderDAL.createRecord(order);
                    sale.setPrice(sale.getPrice() + sale.orders.Sum(order => orderDAL.getProductPrice(order)));
                    saleDAL.updateRecord(sale);
                    chooseChocolate = false;
                    chooseMekupelet = false;
                    chooseVanilla = false;
                }
                #endregion
                #region incomplete sales
                for (int i = 0; i < 30; i++)
                {
                    sale = new(dates[r.Next(0, dates.Length - 1)]);
                    saleDAL.createRecord(sale);
                    sale.setId(saleDAL.GetPK());
                }
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[fillIncompleteAndCompleteSales] Error: {ex.Message}");
            }

            return this;
        }
    }

}
