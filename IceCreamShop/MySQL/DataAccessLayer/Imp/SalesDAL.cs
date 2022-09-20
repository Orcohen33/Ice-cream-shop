using IceCreamShop.MySQL.DataAccessLayer.Factory;
using IceCreamShop.MySQL.DataAccessLayer.Interfaces;
using IceCreamShop.MySQL.Entity;
using MySql.Data.MySqlClient;
#pragma warning disable
namespace IceCreamShop.MySQL.DataAccessLayer.Imp
{
    class SaleDAL : ICrudDAL<Sale>
    {
        private readonly DbContext dbContext = DbContext.Instance;
        #region Crud operations
        public int createRecord(Sale obj)
        {
            int? success = 0;
            try
            {
                dbContext.conn.Open();
                string sql = "INSERT INTO `icecreamshop`.`Sales` (Order_date, price) VALUES (@Order_date, @price);";
                MySqlCommand cmd = new MySqlCommand(sql, dbContext.conn);
                cmd.Parameters.AddWithValue("@Order_date", obj.Order_date);
                cmd.Parameters.AddWithValue("@price", obj.Price);
                success = cmd.ExecuteNonQuery();
            }
            catch (Exception err) when (err is MySqlException)
            {
                Console.WriteLine("[CreateRecord] Error: " + err.Message);
            }
            finally
            {
                dbContext.conn.Close();
            }
            return (int)(success is null ? 0 : success);
        }

        public int deleteRecord(Sale obj)
        {
            int success = 0;
            try
            {
                dbContext.conn.Open();
                string sql = "DELETE FROM `icecreamshop`.`Sales` WHERE sid = @id";
                MySqlCommand cmd = new MySqlCommand(sql, dbContext.conn);
                cmd.Parameters.AddWithValue("@id", obj.Id);
                success = cmd.ExecuteNonQuery();
            }
            catch (Exception err) when (err is MySqlException)
            {
                Console.WriteLine("[DeleteRecord] Error: " + err.Message);
            }
            finally
            {
                dbContext.conn.Close();
            }
            return success;
        }

        public int deleteRecord(int id)
        {
            int success = 0;
            try
            {
                dbContext.conn.Open();
                string sql = "DELETE FROM `icecreamshop`.`Sales` WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(sql, dbContext.conn);
                cmd.Parameters.AddWithValue("@id", id);
                success = cmd.ExecuteNonQuery();
            }
            catch (Exception err) when (err is MySqlException)
            {
                Console.WriteLine("[DeleteRecord] Error: " + err.Message);
            }
            finally
            {
                dbContext.conn.Close();
            }
            return success;
        }

        public IList<Sale> readAll()
        {
            IList<Sale> sales = new List<Sale>();
            try
            {
                dbContext.conn.Open();
                string sql = "SELECT * FROM `icecreamshop`.`Sales`";
                MySqlCommand cmd = new MySqlCommand(sql, dbContext.conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Sale sale = new Sale();
                    sale.Id = reader.GetInt32(0);
                    sale.Order_date = reader.GetDateTime(1);
                    sale.Price = reader.GetInt32(2);
                    sales.Add(sale);
                }
                reader.Close();
            }
            catch (Exception err) when (err is MySqlException)
            {
                Console.WriteLine("[ReadAll] Error: " + err.Message);
            }
            finally
            {
                dbContext.conn.Close();
            }
            return sales;
        }

        public int updateRecord(int pk, Sale obj)
        {
            throw new NotImplementedException();
        }
        public int updateRecord(Sale obj)
        {
            int success = 0;
            try
            {
                dbContext.conn.Open();
                string sql = "UPDATE `icecreamshop`.`Sales` SET Order_date = @Order_date, price = @price WHERE sid = @id";
                MySqlCommand cmd = new MySqlCommand(sql, dbContext.conn);
                cmd.Parameters.AddWithValue("@id", obj.Id);
                cmd.Parameters.AddWithValue("@Order_date", obj.Order_date);
                cmd.Parameters.AddWithValue("@price", obj.Price);
                success = cmd.ExecuteNonQuery();
            }
            catch (Exception err) when (err is MySqlException)
            {
                Console.WriteLine("[updateRecord] Error: " + err.Message);
            }
            finally
            {
                dbContext.conn.Close();
            }
            return success;
        }
        #endregion
        #region Get primary key
        public int GetPK()
        {
            int pk = 0;
            try
            {
                dbContext.conn.Open();
                string sql = "SELECT MAX(sid) AS sid FROM `icecreamshop`.`Sales`;";
                MySqlCommand cmd = new MySqlCommand(sql, dbContext.conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                pk = int.Parse(reader["sid"].ToString());
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                dbContext.conn.Close();
            }
            return pk;
        }
        #endregion
    }
}
