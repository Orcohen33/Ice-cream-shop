using IceCreamShop.MySQL.DataAccessLayer.Factory;
using IceCreamShop.MySQL.DataAccessLayer.Interfaces;
using IceCreamShop.MySQL.Entity;
using MySql.Data.MySqlClient;

namespace IceCreamShop.MySQL.DataAccessLayer.Imp
{
    class IngredientDAL : ICrudDAL<Ingredient>
    {
        private readonly DbContext dbContext = DbContext.Instance;
        #region Crud operations
        public int createRecord(Ingredient obj)
        {
            try
            {
                dbContext.conn.Open();
                string sql = "INSERT INTO ingredient (name, type, price) VALUES (@name, @type,@price)";
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                new MySqlParameter("@name", obj.Name),
                new MySqlParameter("@type", obj.Type),
                new MySqlParameter("@price", obj.Price)
                };
                return new MySqlCommand(sql, dbContext.conn).ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
            finally
            {
                dbContext.conn.Close();
            }
        }

        public int deleteRecord(Ingredient obj)
        {
            try
            {
                dbContext.conn.Open();
                string sql = "DELETE FROM ingredient WHERE id = @id";
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                new MySqlParameter("@id", obj.Id)
                };
                return new MySqlCommand(sql, dbContext.conn).ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
            finally
            {
                dbContext.conn.Close();
            }
        }

        public int deleteRecord(int pk)
        {
            try
            {
                string sql = "DELETE FROM ingredient WHERE id = @id";
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                new MySqlParameter("@id", pk)
                };
                return new MySqlCommand(sql, dbContext.conn).ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                return 0;

            }
            finally
            {
                dbContext.conn.Close();
            }
        }
        public int updateRecord(int pk, Ingredient obj)
        {
            throw new NotImplementedException();
        }

        public int updateRecord(Ingredient obj)
        {
            try
            {
                string sql = "UPDATE ingredient SET name = @name, price = @price WHERE id = @id";
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                new MySqlParameter("@name", obj.Name),
                new MySqlParameter("@price", obj.Price),
                new MySqlParameter("@id", obj.Id)
                };
                return new MySqlCommand(sql, dbContext.conn).ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
            finally
            {
                dbContext.conn.Close();
            }
        }

        public IList<Ingredient> readAll()
        {
            try
            {
                dbContext.conn.Open();
                string sql = "SELECT * FROM `icecreamshop`.`ingredients`";
                MySqlDataReader reader = new MySqlCommand(sql, dbContext.conn).ExecuteReader();
                IList<Ingredient> ingredients = new List<Ingredient>();
                while (reader.Read())
                {
                    Ingredient ingredient = new Ingredient(
                        (int)reader.GetInt64(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetInt32(3)
                        );
                    ingredients.Add(ingredient);
                }
                reader.Close();
                dbContext.conn.Close();
                return ingredients;
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            finally
            {
                dbContext.conn.Close();
            }
        }
        #endregion
        public Ingredient GetIngredientByID(int id)
        {
            Ingredient ans = null;
            try
            {
                dbContext.conn.Open();
                string sql = "SELECT * FROM `icecreamshop`.`ingredients` WHERE iid = @id;";
                MySqlCommand cmd = new MySqlCommand(sql, dbContext.conn);
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                ans = new Ingredient(
                    (int)reader.GetInt64(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetInt32(3)
                    );
                reader.Close();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message.ToString());
            }
            finally
            {
                dbContext.conn.Close();
            }

            return ans;
        }
        public int GetIngredientID(string name, string type)
        {
            dbContext.conn.Open();
            string sql = "SELECT iid FROM `icecreamshop`.`ingredients` WHERE name = @name AND type = @type";
            MySqlCommand cmd = new MySqlCommand(sql, dbContext.conn);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@type", type);
            MySqlDataReader reader = cmd.ExecuteReader();
            int iid = 0;
            while (reader.Read())
            {
                iid = reader.GetInt32(0);
            }
            reader.Close();
            dbContext.conn.Close();
            return iid;
        }

        public IList<Ingredient> GetIngredientsListByType(string type)
        {
            IList<Ingredient> ingredients = new List<Ingredient>(); // return value
            try
            {
                dbContext.conn.Open();
                string sql = "SELECT * FROM `icecreamshop`.`ingredients` s WHERE s.type = @type;";
                MySqlCommand cmd = new MySqlCommand(sql, dbContext.conn);
                cmd.Parameters.AddWithValue("@type", type);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ingredients.Add(new(
                        int.Parse(reader[0].ToString()),
                        reader[1].ToString(),
                        reader[2].ToString(),
                        int.Parse(reader[3].ToString())
                        ));
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                dbContext.conn.Close();
            }
            return ingredients;
        }

    }
}
