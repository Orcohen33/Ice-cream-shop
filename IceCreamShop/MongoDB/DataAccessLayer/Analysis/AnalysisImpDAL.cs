using ConsoleTables;
using IceCreamShop.MongoDB.DataAccessLayer.Factory;
using IceCreamShop.MongoDB.Entity;
using MongoDB.Driver;
#pragma warning disable
namespace IceCreamShop.MongoDB.DataAccessLayer.Analysis
{
    internal class AnalysisImpDAL
    {
        private readonly IMongoDatabase dbConnection = MongoDBInstance.GetMongoDatabase;
        public string customerInvoice(int sid)
        {
            string result = "";
            var collection = dbConnection.GetCollection<Sale>("Sales");
            try
            {
                if (sid > collection.CountDocuments(x => x.price != null || x.price != 0))
                {
                    throw new IndexOutOfRangeException("Sale ID does not exist");
                }
                var sale = collection.Find(_ => true).ToList()[sid];
                result = "------------------------------ Customer invoice ------------------------------\n";
                result += "ID: " + sid + "\n";
                result += "Order date: " + sale.order_date.ToShortDateString() + "\n";
                result += "Price: " + sale.price + "\n";
                result += "Ingredients: \n";
                foreach (var ingredient in sale.ingredients)
                {
                    result += "\t" + ingredient.name + "\n";
                }
                return result;
            }
            catch (Exception err)
            {
                Console.WriteLine($"[customerInvoice] Error: {err.Message}");
            }
            return "";
        }

        internal string endOfDayReport(string date)
        {
            var table = new ConsoleTable("date", "total_sales", "total_price", "average_price");
            var collection = dbConnection.GetCollection<Sale>("Sales");
            // valid string date to parse
            if (DateTime.TryParse(date, out DateTime dateTime))
            {
                var sales = collection.Find(_ => true).ToList();
                var group = sales.GroupBy(x => x.order_date.Date)
                     .FirstOrDefault(x => x.Key.Date.ToShortDateString().Equals(dateTime.ToShortDateString()));
                if (group != null)
                    table.AddRow(group.Key.ToShortDateString(), group.Count(), group.Sum(x => x.price), group.Average(x => x.price));
                string title = "------------------------------ End of day report ------------------------------\n";
                return title + table.ToString() + "\n";
            }
            else
            {
                return "[endOfDayReport] Error: Date is not valid\n";
            }

        }

        internal string allEndOfDayReports()
        {
            var table = new ConsoleTable("date", "total_sales", "total_price", "average_price");
            var collection = dbConnection.GetCollection<Sale>("Sales");
            var sales = collection.Find(sale => !(sale.price.Equals(0) || sale.price.Equals(null))).ToList();
            sales.GroupBy(sale => sale.order_date.Date).ToList().ForEach(group =>
            {
                table.AddRow(group.Key.ToShortDateString(), group.Count(), group.Sum(x => x.price), group.Average(x => x.price));
            });
            string title = "----------------- All end of day reports -----------------\n";
            string res = "\nTotal sales: " + sales.Count + "\n";
            return title + table.ToString() + res;
        }

        internal string showAllIncompleteSales()
        {
            var table = new ConsoleTable("date", "total_incomplete_sales");
            var collection = dbConnection.GetCollection<Sale>("Sales");
            var sales = collection.Find(sale => sale.price.Equals(0) || sale.price.Equals(null)).ToList();
            sales.GroupBy(sale => sale.order_date.Date).ToList().ForEach(group =>
            {
                table.AddRow(group.Key.ToShortDateString(), group.Count());
            });
            string title = "----------------- All incomplete sales -----------------\n";
            string res = "\nTotal incompleted sales: " + sales.Count() + "\n";
            return title + table.ToString() + res;
        }

        internal string mostCommonIngredientNTaste()
        {
            var table = new ConsoleTable("name", "count", "type");
            var collection = dbConnection.GetCollection<Sale>("Sales");

            var iceCreams = collection.Find(_ => true)
                                      .ToList()
                                      .SelectMany(x => x.ingredients)
                                      .Where(x => x.type.Equals("IceCream"))
                                      .ToList();
            var mostCommonIceCream = iceCreams.GroupBy(x => x.name).OrderByDescending(x => x.Count()).FirstOrDefault();

            var toppings = collection.Find(_ => true)
                                     .ToList()
                                     .SelectMany(x => x.ingredients)
                                     .Where(x => x.type.Equals("Topping"))
                                     .ToList();
            var mostCommonTopping = toppings.GroupBy(x => x.name).OrderByDescending(x => x.Count()).FirstOrDefault();

            table.AddRow(mostCommonIceCream.Key, mostCommonIceCream.Count(), "IceCream");
            table.AddRow(mostCommonTopping.Key, mostCommonTopping.Count(), "Topping");

            return table.ToString() + "\n";
        }
    }
}
