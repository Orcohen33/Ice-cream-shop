using IceCreamShop.MySQL.DataAccessLayer.Factory;
using IceCreamShop.MySQL.DataAccessLayer.Interfaces;
using IceCreamShop.MySQL.Entity;
using MySql.Data.MySqlClient;

namespace IceCreamShop.MySQL.DataAccessLayer.Imp
{
    internal class OrderDAL : ICrudDAL<Order>
    {
        private readonly DbContext dbContext = DbContext.Instance;
        #region Crud operations
        public int createRecord(Order obj)
        {
            int success = 0;
            try
            {
                dbContext.conn.Open();
                string sql = "INSERT INTO `IceCreamStore`.`orders` (sid, iid) VALUES (@SaleId, @IngredientId)";
                MySqlCommand cmd = new MySqlCommand(sql, dbContext.conn);
                cmd.Parameters.AddWithValue("@SaleId", obj.SaleId);
                cmd.Parameters.AddWithValue("@IngredientId", obj.IngredientId);
                success = cmd.ExecuteNonQuery();
            }
            catch (Exception err) when (err is MySqlException)
            {
                Console.WriteLine("[OrderDAL] Error: " + err.Message);
            }
            finally
            {
                dbContext.conn.Close();
            }
            return success;
        }

        public int deleteRecord(Order obj)
        {
            int success = 0;
            try
            {

                dbContext.conn.Open();
                string sql = "DELETE FROM Orders WHERE sid = @SaleId AND iid = @IngredientId";
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                new MySqlParameter("@SaleId", obj.SaleId),
                new MySqlParameter("@IngredientId", obj.IngredientId)
                };
                success = new MySqlCommand(sql, dbContext.conn).ExecuteNonQuery();
            }
            catch (Exception err) when (err is MySqlException)
            {
                Console.WriteLine("[deleteRecord] Error: " + err.Message);
            }
            finally
            {
                dbContext.conn.Close();

            }
            return success;
        }

        public int deleteRecord(int pk)
        {
            int success = 0;
            try
            {
                dbContext.conn.Open();
                string sql = "DELETE FROM Orders WHERE sid = @SaleId";
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@SaleId", pk)
                };
                success = new MySqlCommand(sql, dbContext.conn).ExecuteNonQuery();
            }
            catch (Exception err) when (err is MySqlException)
            {
                Console.WriteLine("[deleteRecord] Error: " + err.Message);
            }
            finally
            {
                dbContext.conn.Close();
            }
            return success;
        }

        public IList<Order> readAll()
        {
            IList<Order> Orders = new List<Order>();
            try
            {
                dbContext.conn.Open();
                string sql = "SELECT * FROM Orders";
                MySqlDataReader reader = new MySqlCommand(sql, dbContext.conn).ExecuteReader();
                while (reader.Read())
                {
                    Order order = new Order(
                        reader.GetInt32(0),
                        reader.GetInt32(1)
                        );
                    Orders.Add(order);
                }
                reader.Close();
            }
            catch (Exception err) when (err is MySqlException)
            {
                Console.WriteLine("[readAll] Error: " + err.Message);
            }
            finally
            {
                dbContext.conn.Close();
            }
            return Orders;
        }

        public int updateRecord(int pk, Order obj)
        {
            throw new NotImplementedException();
        }

        public int updateRecord(Order obj)
        {
            throw new NotImplementedException();
        }
        #endregion
        public int getProductPrice(Order x)
        {
            int price = 0;
            try
            {
                dbContext.conn.Open();
                string sql = "SELECT price FROM `IceCreamStore`.`ingredients` WHERE iid = @IngredientId";
                MySqlCommand cmd = new MySqlCommand(sql, dbContext.conn);
                cmd.Parameters.AddWithValue("@IngredientId", x.IngredientId);
                price = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception err) when (err is MySqlError || err is MySqlException)
            {
                Console.WriteLine("[getProductPrice] Error: " + err.Message);
            }
            finally
            {
                dbContext.conn.Close();
            }
            return price;
        }
    }
}
