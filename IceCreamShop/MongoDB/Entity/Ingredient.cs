using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

#pragma warning disable
namespace IceCreamShop.MongoDB.Entity
{
    public class Ingredient
    {
        [BsonId]
        public ObjectId _id { get; set; }
        [BsonElement("name")]
        public string name { get; set; }
        [BsonElement("type")]
        public string type { get; set; }
        [BsonElement("price")]
        public int price { get; set; }

        public override string? ToString()
        {
            return $"\tName: {name}\n\tType: {type}\n\tPrice: {price}";
        }
    }
}
