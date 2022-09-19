using MySql.Data.MySqlClient;

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

                "INSERT INTO icecreamshop.ingredients(`name`,`type`,`price`) VALUES(\"RegularCup\", \"Box\", 0);" +
                "INSERT INTO icecreamshop.ingredients(`name`,`type`,`price`) VALUES(\"SpecialCup\", \"Box\", 2);" +
                "INSERT INTO icecreamshop.ingredients(`name`,`type`,`price`) VALUES(\"Box\", \"Box\", 5);" +

                "INSERT INTO icecreamshop.ingredients(`name`,`type`,`price`) VALUES(\"Chocolate\", \"Topping\", 2);" +
                "INSERT INTO icecreamshop.ingredients(`name`,`type`,`price`) VALUES(\"Maple\", \"Topping\", 2);" +
                "INSERT INTO icecreamshop.ingredients(`name`,`type`,`price`) VALUES(\"Peanut\", \"Topping\", 2);";

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
            return this;
        }

        // #TODO: Complete this method
        public void fillIncompleteAndCompleteSales()
        {
            string sql = "INSERT INTO icecreamshop.ingredients(`order_date`, `price`) VALUES(@date, @price);";
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
