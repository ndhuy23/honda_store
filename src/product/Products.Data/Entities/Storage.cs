using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProductService.Data.Entities
{
    public class Storage
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]

        public Guid Id { get; set; }
        [BsonRepresentation(BsonType.String)]

        public Guid ProductId { get; set; }
        [BsonRepresentation(BsonType.String)]

        public Guid ColorId { get; set; }

        public int Quantity { get; set; }

        public List<string> Images { get; set; }

    }
}
