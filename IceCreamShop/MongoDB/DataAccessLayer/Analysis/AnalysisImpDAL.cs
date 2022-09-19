﻿using IceCreamShop.MongoDB.DataAccessLayer.Factory;
using IceCreamShop.MongoDB.Entity;
using MongoDB.Driver;

namespace IceCreamShop.MongoDB.DataAccessLayer.Analysis
{
    internal class AnalysisImpDAL
    {
        private readonly IMongoDatabase dbConnection = MongoDBInstance.GetMongoDatabase;
        public string customerInvoice(int sid)
        {
            var collection = dbConnection.GetCollection<Sale>("Sales");
            try
            {
                if (sid > collection.CountDocuments(x => x.price != null || x.price != 0))
                {
                    throw new IndexOutOfRangeException("Sale ID does not exist");
                }
                return collection.Find(_ => true).ToList()[sid].ToString();
            }
            catch (Exception err)
            {
                Console.WriteLine($"[customerInvoice] Error: {err.Message}");
            }

            return "";
        }

        internal string endOfDayReport(string date)
        {
            var collection = dbConnection.GetCollection<Sale>("Sales");
            var obj = collection.Find(sale => sale.order_date.Equals(date)).ToList();
            return obj.ToString();
        }

        internal string allEndOfDayReports()
        {
            var collection = dbConnection.GetCollection<Sale>("Sales");
            var obj = collection.Find(sale => sale.price != null).ToList();
            string res = "----------------- All end of day reports -----------------\n";
            foreach (Sale o in obj)
            {
                res += o + "\n";
            }
            res += "Total sales: " + obj.Count();
            return res;
        }

        internal string showAllIncompleteSales()
        {
            var collection = dbConnection.GetCollection<Sale>("Sales");
            var obj = collection.Find(sale => sale.price.Equals(0) || sale.price.Equals(null)).ToList();
            string res = "----------------- All end of day reports -----------------\n";
            foreach (Sale o in obj)
            {
                res += o + "\n";
            }
            res += "Total incompleted sales: " + obj.Count();
            return res;
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