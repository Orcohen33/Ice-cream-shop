using IceCreamShop.MongoDB.DataAccessLayer.Factory;
using IceCreamShop.MongoDB.DataAccessLayer.Interfaces;
using IceCreamShop.MongoDB.Entity;
using MongoDB.Bson;
using MongoDB.Driver;
#pragma warning disable
namespace IceCreamShop.MongoDB.DataAccessLayer.Imp
{
    public class SaleDAL : ICrudDAL<Sale>
    {
        private readonly IMongoDatabase dbConnection = MongoDBInstance.GetMongoDatabase;
        #region CRUD operations
        public void CreateDocument(Sale sale)
        {
            try
            {
                var collection = dbConnection.GetCollection<Sale>("Sales");
                collection.InsertOne(sale);
            }
            catch (Exception err) when (err.InnerException != null || err is MongoWriteException)
            {
                Console.WriteLine($"[CreateDocument] Error: {err.Message}");
            }
        }

        public void DeleteDocument(Sale sale)
        {
            try
            {
                var collection = dbConnection.GetCollection<Sale>("Sales");
                var deleteFilter = Builders<Sale>.Filter.Eq("_id", sale._id);
                collection.DeleteOne(deleteFilter);
            }
            catch (Exception err) when (err.InnerException != null || err is MongoException)
            {
                Console.WriteLine($"[DeleteDocument] Error: {err.Message}");
            }
        }

        public ICollection<Sale> ReadAll()
        {
            try
            {
                return dbConnection.GetCollection<Sale>("Sales").Find(_ => true).ToList();
            }
            catch (Exception err) when (err.InnerException != null || err is MongoException)
            {
                Console.WriteLine($"[ReadAll] Error: {err.Message}");
            }
            return null;
        }

        public Sale ReadDocument(int id)
        {
            try
            {
                var collection = dbConnection.GetCollection<Sale>("Sales");
                var obj = collection.Find(sale => sale._id.Equals(id)).ToList();
                return obj[0];

            }
            catch (Exception err) when (err.InnerException != null || err is MongoException)
            {
                Console.WriteLine($"[ReadDocument] Error: {err.Message}");
            }
            return null;
        }

        public void UpdateDocument(Sale sale)
        {
            try
            {
                var collection = dbConnection.GetCollection<Sale>("Sales");
                var updateFilter = Builders<Sale>.Filter
                                                      .Eq("_id",
                                                       sale._id);
                var update = Builders<Sale>.Update
                    .Set("order_date", sale.order_date)
                    .Set("price", sale.price)
                    .Set("ingredients", sale.ingredients);
                collection.UpdateOne(updateFilter, update);
            }
            catch (Exception err) when (err.InnerException != null || err is MongoException)
            {
                Console.WriteLine($"[UpdateDocument] Error: {err.Message}");
            }
        }
        #endregion
        public ObjectId GetLastObjectIdInserted()
        {
            var collection = dbConnection.GetCollection<Sale>("Sales");
            var obj = collection.Find(sale => true).ToList();
            return obj[obj.Count - 1]._id;
        }
    }

}
