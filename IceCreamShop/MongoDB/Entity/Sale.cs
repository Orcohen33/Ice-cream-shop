using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IceCreamShop.MongoDB.Entity
{
    public class Sale
    {
        [BsonId]
        //[BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]

        public ObjectId _id { get; set; }
        [BsonElement]

        public DateTime order_date { get; set; }
        [BsonElement]

        public int? price { get; set; }
        [BsonElement]

        public ICollection<Ingredient>? ingredients { get; set; }

        public override string? ToString()
        {
            return $"{_id}\n\t{order_date}\n\t{price}";
        }
    }
}




/*
   Example of the `Sales` collection
{
    "sid": 1,
    "order_date": "2021-06-01T00:00:00",
    "price": 7,
    "ingredients":[
                    "1": {
                        "name": "RegularCup",
                        "type": "Box",
                        "price": 0
                    },
                    "2": {
                        "name": "Chocolate",
                        "type": "IceCream",
                        "price": 7
                  ],
    "sid": 2,
    "order_date": "2021-06-01T00:00:00",
    "price": ,
    "ingredients":[],

    "sid": 3,
    "order_date": "2021-06-01T00:00:00",
    "price": ,
    "ingredients":[],
}


 */

