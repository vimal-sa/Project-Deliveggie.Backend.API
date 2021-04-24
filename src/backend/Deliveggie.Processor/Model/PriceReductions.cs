using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Deliveggie.Processor.Model
{
    public class PriceReductions
    {
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.String)]
        public ObjectId id { get; set; }

        [BsonElement("DayOfWeek")]
        [BsonRepresentation(BsonType.Int64)]
        public int DayOfWeek { get; set; }

        [BsonElement("Reduction")]
        [BsonRepresentation(BsonType.Double)]
        public double Reduction { get; set; }
    }
}
