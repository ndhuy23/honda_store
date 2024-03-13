using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProductService.Data.Entities
{
    public class Preferential
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]

        public Guid Id { get; set; }

        public string Content { get; set; }
    }
}
