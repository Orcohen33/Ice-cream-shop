using IceCreamShop.MySQL.DataAccessLayer.Interfaces;
using MySql.Data.MySqlClient;

namespace IceCreamShop.MySQL.DataAccessLayer.Factory
{
    class MySQLCreateNFillData : ICreateNFillDAL
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

        #endregion
        public void MySQLcreateTables()
        {
            try
            {
                dbContext.conn.Open();

                string sql = "DROP DATABASE IF EXISTS icecreamstore;";
                MySqlCommand cmd = new MySqlCommand(sql, dbContext.conn);
                cmd.ExecuteNonQuery();

                Console.WriteLine("Deleted previous database");
                // create IceCreamStore schema
                sql = "CREATE DATABASE IceCreamStore;";
                cmd = new MySqlCommand(sql, dbContext.conn);
                cmd.ExecuteNonQuery();

                // create Sales
                sql = "CREATE TABLE `IceCreamStore`.`Sales` (" +
                    "`sid` INT NOT NULL AUTO_INCREMENT, " +
                    "`order_date` DATE NOT NULL," +
                    "`price` INT NULL," +
                    "PRIMARY KEY (`sid`));";

                cmd = new MySqlCommand(sql, dbContext.conn);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Created Sales");

                // create Ingredient
                sql = "CREATE TABLE `IceCreamStore`.`ingredients` (" +
                    "`iid` INT NOT NULL AUTO_INCREMENT, " +
                    "`name` VARCHAR(45) NOT NULL," +
                    "`type` VARCHAR(45) NOT NULL," +
                    "`price` INT NOT NULL," +
                    "PRIMARY KEY (`iid`));";

                cmd = new MySqlCommand(sql, dbContext.conn);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Created ingredients");

                // create orders
                sql = "CREATE TABLE `IceCreamStore`.`Orders` (" +
                    "`id` INT NOT NULL AUTO_INCREMENT PRIMARY KEY, " +
                    "`sid` INT NOT NULL," +
                    "`iid` INT NOT NULL," +
                 "CONSTRAINT fk_OrdersSid FOREIGN KEY (sid) " +
                "REFERENCES IceCreamStore.Sales(sid), " +
                "CONSTRAINT fk_OrdersIid FOREIGN KEY(iid) " +
                "REFERENCES IceCreamStore.ingredients(iid));";
                cmd = new MySqlCommand(sql, dbContext.conn);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Created orders");

                dbContext.conn.Close();
            }
            catch (Exception ex) when (ex is MySqlException)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void MySQLfillIngredients()
        {
            try
            {
                dbContext.conn.Open();
                string sql = "INSERT INTO icecreamstore.ingredients(`name`, `type`,`price`) VALUES(\"Chocolate\", \"IceCream\", 6);" +
                "INSERT INTO icecreamstore.ingredients(`name`, `type`,`price`) VALUES(\"Mekupelet\", \"IceCream\", 6);" +
                "INSERT INTO icecreamstore.ingredients(`name`, `type`,`price`) VALUES(\"Avocado\", \"IceCream\", 6);" +
                "INSERT INTO icecreamstore.ingredients(`name`, `type`,`price`) VALUES(\"Vanilla\", \"IceCream\", 6);" +
                "INSERT INTO icecreamstore.ingredients(`name`, `type`,`price`) VALUES(\"Loacker\", \"IceCream\", 6);" +
                "INSERT INTO icecreamstore.ingredients(`name`, `type`,`price`) VALUES(\"Oreo\", \"IceCream\", 6);" +

                "INSERT INTO icecreamstore.ingredients(`name`,`type`,`price`) VALUES(\"RegularCup\", \"Box\", 0);" +
                "INSERT INTO icecreamstore.ingredients(`name`,`type`,`price`) VALUES(\"SpecialCup\", \"Box\", 2);" +
                "INSERT INTO icecreamstore.ingredients(`name`,`type`,`price`) VALUES(\"Box\", \"Box\", 5);" +

                "INSERT INTO icecreamstore.ingredients(`name`,`type`,`price`) VALUES(\"Chocolate\", \"Topping\", 2);" +
                "INSERT INTO icecreamstore.ingredients(`name`,`type`,`price`) VALUES(\"Maple\", \"Topping\", 2);" +
                "INSERT INTO icecreamstore.ingredients(`name`,`type`,`price`) VALUES(\"Peanut\", \"Topping\", 2);";

                MySqlCommand cmd = new MySqlCommand(sql, dbContext.conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception err)
            {
                err.Message.ToString();
            }
            finally
            {
                dbContext.conn.Close();
            }
        }

        // #TODO: Complete this method
        public void fillIncompleteAndCompleteSales()
        {
            string sql = "INSERT INTO icecreamstore.ingredients(`order_date`, `price`) VALUES(@date, @price);";
            Random r = new();
            int randNumber = r.Next(0, dates.Length);
            try
            {
                dbContext.conn.Open();

                for (int i = 0; i < dates.Length; i++)
                {
                    MySqlCommand cmd = new MySqlCommand(sql, dbContext.conn);
                    cmd.Parameters.AddWithValue("@date", dates[randNumber]);
                    cmd.Parameters.AddWithValue("@price", randNumber);
                    cmd.ExecuteNonQuery();
                    randNumber = r.Next(0, dates.Length);
                }
            }
            catch (Exception err)
            {
                err.Message.ToString();
            }
            finally
            {
                dbContext.conn.Close();
            }

        }
    }

}
