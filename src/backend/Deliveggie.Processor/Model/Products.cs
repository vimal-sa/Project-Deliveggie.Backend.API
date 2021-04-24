using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Deliveggie.Processor.Model
{
    public class Products
    {
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.String)]
        public ObjectId id { get; set; }

        [BsonElement("Name")]
        [BsonRepresentation(BsonType.String)]
        public string Name { get; set; }

        [BsonElement("EntryDate")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime EntryDate { get; set; }

        [BsonElement("Price")]
        [BsonRepresentation(BsonType.Double)]
        public double Price { get; set; }
    }
}
