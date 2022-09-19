using MySql.Data.MySqlClient;

namespace IceCreamShop.MySQL.DataAccessLayer.Factory
{
    public sealed class DbContext
    {
        private static readonly Lazy<DbContext> lazy =
            new Lazy<DbContext>(() => new DbContext());

        public static DbContext Instance { get { return lazy.Value; } }

        public MySqlConnection? conn;
        public string sql = "";
        private DbContext()
        {
            try
            {
                /* Fill your connection details */
                const string connStr = "server=localhost;user=root;port=3306;password=root";
                conn = new MySqlConnection(connStr);
                Console.WriteLine("[DbContext] Connecting to MySQL...");
            }
            catch (Exception err)
            {
                Console.WriteLine($"{err.Message}");
            }
        }
    }
}
