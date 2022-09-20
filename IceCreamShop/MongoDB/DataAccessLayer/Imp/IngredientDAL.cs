using IceCreamShop.MongoDB.DataAccessLayer.Factory;
using IceCreamShop.MongoDB.DataAccessLayer.Interfaces;
using IceCreamShop.MongoDB.Entity;
using MongoDB.Driver;
#pragma warning disable
namespace IceCreamShop.MongoDB.DataAccessLayer.Imp
{
    internal class IngredientDAL : ICrudDAL<Ingredient>
    {
        private readonly IMongoDatabase dbConnection = MongoDBInstance.GetMongoDatabase;
        #region CRUD operations
        public void CreateDocument(Ingredient ingredient)
        {
            try
            {
                var collection = dbConnection.GetCollection<Ingredient>("Ingredients");
                collection.InsertOne(ingredient);
                Console.WriteLine("[CreateDocument] Inserted");
            }
            catch (Exception err) when (err is MongoException || err is MongoWriteException)
            {
                Console.WriteLine("[CreateDocument] Error: " + err.Message);
            }
        }

        public void DeleteDocument(Ingredient ingredient)
        {
            try
            {
                var collection = dbConnection.GetCollection<Ingredient>("Ingredients");
                var deleteFilter = Builders<Ingredient>.Filter.Eq("_id", ingredient._id);
                collection.DeleteOne(deleteFilter);
                Console.WriteLine("[DeleteDocument] Deleted");
            }
            catch (Exception err) when (err is MongoException || err is MongoWriteException)
            {
                Console.WriteLine("[DeleteDocument] Error: " + err.Message);
            }
        }

        public void DeleteDocument(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Ingredient> ReadAll()
        {
            try
            {
                var collection = dbConnection.GetCollection<Ingredient>("Ingredients");
                return collection.Find(_ => true).ToList();
            }
            catch (Exception err) when (err is MongoException || err is MongoWriteException)
            {
                Console.WriteLine("[ReadAll] Error: " + err.Message);
            }
            return null;
        }

        public ICollection<Ingredient> ReadAllByType(string type)
        {
            try
            {
                var collection = dbConnection.GetCollection<Ingredient>("Ingredients");
                var obj = collection.Find(ingredient => ingredient.type.Equals(type)).ToList();
                return obj;
            }
            catch (Exception err) when (err is MongoException || err is MongoWriteException)
            {
                Console.WriteLine("[ReadAllByType] Error: " + err.Message);
            }
            return null;
        }

        public Ingredient ReadDocument(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateDocument(Ingredient ingredient)
        {
            try
            {
                var collection = dbConnection.GetCollection<Ingredient>("Ingredients");
                var updateFilter = Builders<Ingredient>.Filter.Eq("_id", ingredient._id);
                var update = Builders<Ingredient>.Update
                    .Set("name", ingredient.name)
                    .Set("type", ingredient.type)
                    .Set("price", ingredient.price);
                collection.UpdateOne(updateFilter, update);
                Console.WriteLine("[UpdateDocument] Updated");
            }
            catch (Exception err) when (err.InnerException != null || err is MongoException || err is MongoWriteException)
            {
                Console.WriteLine("[UpdateDocument] Error: " + err.Message);
            }
        }
        #endregion
        public Ingredient? GetIngredientByName(string name, string type)
        {
            try
            {
                var collection = dbConnection.GetCollection<Ingredient>("Ingredients");
                var obj = collection.Find(ingredient => ingredient.name.Equals(name) && ingredient.type.Equals(type)).ToList();
                return obj[0];
            }
            catch (Exception err) when (err is MongoException || err is MongoWriteException)
            {
                Console.WriteLine("[GetIngredientByName] Error: " + err.Message);
            }
            return null;
        }

    }
}
