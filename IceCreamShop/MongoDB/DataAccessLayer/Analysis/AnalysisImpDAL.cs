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
            var collection = dbConnection.GetCollection<Sale>("Sales");

            var mostIngredients = collection
                .Find(x => true).ToList();

            IDictionary<string, IDictionary<string, int>> keyValuePairs = new Dictionary<string, IDictionary<string, int>>();
            keyValuePairs.Add("IceCream", new Dictionary<string, int>());
            keyValuePairs.Add("Topping", new Dictionary<string, int>());
            keyValuePairs.Add("Box", new Dictionary<string, int>());

            foreach (Sale sale in mostIngredients)
            {
                foreach (var mongoIngredient in sale.ingredients)
                {
                    switch (mongoIngredient.type)
                    {
                        case "IceCream":
                            if (!keyValuePairs["IceCream"].ContainsKey(mongoIngredient.name))
                            {
                                keyValuePairs["IceCream"].Add(mongoIngredient.name, 1);
                            }
                            keyValuePairs["IceCream"][mongoIngredient.name] += 1;
                            break;
                        case "Topping":
                            if (!keyValuePairs["Topping"].ContainsKey(mongoIngredient.name))
                            {
                                keyValuePairs["Topping"].Add(mongoIngredient.name, 1);
                            }
                            keyValuePairs["Topping"][mongoIngredient.name] += 1;
                            break;
                        case "Box":
                            if (!keyValuePairs["Box"].ContainsKey(mongoIngredient.name))
                            {
                                keyValuePairs["Box"].Add(mongoIngredient.name, 1);
                            }
                            keyValuePairs["Box"][mongoIngredient.name] += 1;
                            break;
                    }
                }
            }
            keyValuePairs.ToList().ForEach(x => Console.WriteLine(x));

            int max = 0;
            string icecream = "";
            foreach (string key in keyValuePairs["IceCream"].Keys)
            {
                if (max < keyValuePairs["IceCream"][key])
                {
                    max = keyValuePairs["IceCream"][key];
                    icecream = key;
                }
            }
            Console.WriteLine($"{icecream} : {max}");
            //var mostIngredients = collection
            //    .Aggregate()
            //    .Group(sale => sale.ingredients, g => new { ingredient = g.Key, count = g.Count() })
            //    .SortByDescending(g => g.count).Limit(1).ToList();

            //// find the most ingredient with type 'IceCream' and print it
            //var mostTaste = collection
            //    .Aggregate()
            //    .Group(sale => sale.ingredients, g => new { ingredient = g.Key, count = g.Count() })
            //    .SortByDescending(g => g.count).Limit(1).ToList();

            var obj = collection.Find(sale => true).ToList();
            return obj[0].ToString();
        }
    }
}
