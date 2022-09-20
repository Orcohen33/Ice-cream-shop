using ConsoleTables;
using IceCreamShop.MySQL.DataAccessLayer.Factory;
using MySql.Data.MySqlClient;
#pragma warning disable
namespace IceCreamShop.MySQL.DataAccessLayer.Analysis
{
    internal class AnalysisImpDAL
    {
        private readonly DbContext dbContext = DbContext.Instance;

        // Customer invoice       
        public string customerInvoice(int sid)
        {
            string result = "Wrong sid";
            try
            {
                dbContext.conn.Open();
                string sql = "SELECT * FROM `icecreamshop`.`sales` WHERE sid = " + sid;
                MySqlCommand cmd = new MySqlCommand(sql, dbContext.conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                result = "------------------------------ Customer invoice ------------------------------\n";
                while (reader.Read())
                {
                    result += "Order Date: " + (DateTime)reader["order_date"] + "\n";
                    result += "price: " + reader["price"] + "\n";
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
            Console.WriteLine("===[EndOfDayReport]===");
            // select sum of all sales, how many sales , avarege of all sales
            var table = new ConsoleTable("date", "total_sales", "total_price", "average_price");
            try
            {
                dbContext.conn.Open();
                string sql = "SELECT order_date ,count(sid) as total_sales, sum(price) as total_price, avg(price) as average_price FROM `icecreamshop`.`sales` WHERE order_date = '" + date + "';";
                MySqlDataReader reader = new MySqlCommand(sql, dbContext.conn).ExecuteReader();
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
            string title = "------------------------------ End of day report ------------------------------\n";
            return title + table.ToString() + "\n";
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
            Console.WriteLine("===[ShowAllIncompleteSales]===");
            // table variable
            var table = new ConsoleTable("id", "date");
            dbContext.conn.Open();
            string sql = "SELECT * FROM `icecreamshop`.`sales` WHERE price IS NULL";
            MySqlDataReader reader = new MySqlCommand(sql, dbContext.conn).ExecuteReader();
            while (reader.Read())
            {
                table.AddRow(reader.GetInt64(0), reader.GetDateTime(1).ToShortDateString());
            }
            reader.Close();
            dbContext.conn.Close();
            string title = "------------------------------ All incomplete sales ------------------------------\n";
            return title + "\n" + table.ToString() + "\n";
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