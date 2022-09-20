using ConsoleTables;
using IceCreamShop.MySQL.DataAccessLayer.Factory;
using IceCreamShop.MySQL.DataAccessLayer.Imp;
using MySql.Data.MySqlClient;
#pragma warning disable
namespace IceCreamShop.MySQL.DataAccessLayer.Analysis
{
    internal class AnalysisImpDAL
    {
        private readonly DbContext dbContext = DbContext.Instance;
        private readonly IngredientDAL ingredientDAL = new();
        // Customer invoice       
        public string customerInvoice(int sid)
        {
            string result = "";
            try
            {
                var ingredients = ingredientDAL.readAll();
                dbContext.conn.Open();
                string sql = "SELECT * FROM `icecreamshop`.`sales` WHERE sid = " + sid;
                MySqlCommand cmd = new MySqlCommand(sql, dbContext.conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (!reader.HasRows) throw new Exception("No such sale");

                result = "------------------------------ Customer invoice ------------------------------\n";
                while (reader.Read())
                {
                    result += "ID: " + (int)reader["sid"] + "\n";
                    result += "Order date: " + (DateTime)reader["order_date"] + "\n";
                    result += "Price: " + reader["price"] + "\n";
                }
                reader.Close();

                sql = "SELECT * FROM `icecreamshop`.`orders` WHERE sid = " + sid;
                cmd = new MySqlCommand(sql, dbContext.conn);
                reader = cmd.ExecuteReader();
                result += "Ingredients:\n";
                while (reader.Read())
                {
                    var ingredient = ingredients.First(x => x.Id.Equals((int)reader["iid"]));
                    result += "\tType: " + ingredient.Type + "\tName: " + ingredient.Name + "\n";
                }
                reader.Close();
            }
            catch (Exception err)
            {
                Console.WriteLine($"{err.Message}");
            }
            finally
            {
                dbContext.conn.Close();
            }
            return result + "\n";
        }
        // End of day report: sales amount, sales volume, average price to sale
        public string endOfDayReport(string date)
        {
            string result = "";
            // select sum of all sales, how many sales , avarege of all sales
            var table = new ConsoleTable("date", "total_sales", "total_price", "average_price");
            try
            {
                dbContext.conn.Open();
                string sql = "SELECT order_date ,count(sid) as total_sales, sum(price) as total_price, avg(price) as average_price FROM `icecreamshop`.`sales` WHERE order_date = '" + date + "';";
                MySqlDataReader reader = new MySqlCommand(sql, dbContext.conn).ExecuteReader();
                if (!reader.HasRows) throw new Exception("No such date");
                result = "------------------------------ End of day report ------------------------------\n";
                while (reader.Read())
                {
                    table.AddRow(reader.GetDateTime(0).ToShortDateString(), reader.GetInt64(1), reader.GetDecimal(2), reader.GetDecimal(3));
                }
                reader.Close();
            }
            catch (Exception err)
            {
                Console.WriteLine($"{err.Message}\nThere is no sales on this date");
            }
            finally
            {
                dbContext.conn.Close();
            }
            return result + table.ToString() + "\n";
        }
        public string allEndOfDayReports()
        {
            Console.WriteLine("===[AllEndOfDayReports]===");
            // select sum of all sales, how many sales , avarege of all sales
            var table = new ConsoleTable("date", "total_sales", "total_price", "average_price");
            try
            {
                dbContext.conn.Open();
                string sql = "SELECT order_date ,count(sid) as total_sales, sum(price) as total_price, avg(price) as average_price FROM `icecreamshop`.`sales` group by order_date;";
                MySqlDataReader reader = new MySqlCommand(sql, dbContext.conn).ExecuteReader();
                while (reader.Read())
                {

                    table.AddRow(reader.GetDateTime(0).ToShortDateString(), reader.GetInt64(1), reader.GetDecimal(2), reader.GetDecimal(3));
                }
                reader.Close();
            }
            catch (Exception err)
            {
                Console.WriteLine($"{err.Message}");
            }
            finally
            {
                dbContext.conn.Close();
            }
            string title = "------------------------------ All end of day reports ------------------------------\n";
            return title + table.ToString() + "\n";
        }

        // Incompleted sales.
        public string showAllIncompleteSales()
        {
            string result = "";
            // table variable
            var table = new ConsoleTable("date", "total_incomplete_sales");
            try
            {
                dbContext.conn.Open();
                string sql = "SELECT order_date ,count(sid) as incomplete_sales " +
                             "FROM `icecreamshop`.`sales` u " +
                             "WHERE u.price is null " +
                             "GROUP BY u.order_date;";
                MySqlDataReader reader = new MySqlCommand(sql, dbContext.conn).ExecuteReader();
                result = "------------------------------ All incomplete sales ------------------------------\n";
                while (reader.Read())
                {
                    table.AddRow(reader.GetDateTime(0).ToShortDateString(), reader.GetInt32(1));
                }
                reader.Close();
            }
            catch (Exception err)
            {
                Console.WriteLine($"[showAllIncompleteSales] Error: {err.Message}");
            }
            finally
            {
                dbContext.conn.Close();
            }
            return result + "\n" + table.ToString() + "\n";
        }
        // The most common ingredient (it is mandatory to use JOIN) and the most common taste.
        public string mostCommonIngredientNTaste()
        {

            var table = new ConsoleTable("name", "type", "Count", "Most");
            try
            {
                dbContext.conn.Open();
                // Most common ingredient query

                string sql = "SELECT ingredients.name as name, ingredients.type as type, count(orders.iid) as count " +
                             "FROM `icecreamshop`.`orders` JOIN `icecreamshop`.`ingredients` " +
                             "ON orders.iid = ingredients.iid " +
                             "GROUP BY orders.iid " +
                             "ORDER BY count DESC LIMIT 1;";
                MySqlDataReader reader = new MySqlCommand(sql, dbContext.conn).ExecuteReader();
                reader.Read();
                table.AddRow(reader.GetString(0), reader.GetString(1), reader.GetInt64(2), "Ingredient");
                reader.Close();

                // Most common taste query
                sql = "SELECT ingredients.name, ingredients.type, count(orders.iid) as count " +
                    "FROM icecreamshop.orders join icecreamshop.ingredients " +
                    "on orders.iid = ingredients.iid " +
                    "WHERE ingredients.type = \"IceCream\" " +
                    "group by orders.iid " +
                    "order by count(orders.iid) desc limit 1; ";
                reader = new MySqlCommand(sql, dbContext.conn).ExecuteReader();
                reader.Read();

                table.AddRow(reader.GetString(0), reader.GetString(1), reader.GetInt64(2), "Taste");
                reader.Close();
            }
            catch (Exception err)
            {
                Console.WriteLine($"{err.Message}");
            }
            finally
            {
                dbContext.conn.Close();
            }
            string title = "------------------------------ Most common ingredient and taset ------------------------------\n";
            return title + table.ToString() + "\n";
        }
    }
}