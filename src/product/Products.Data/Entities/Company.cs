using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ProductService.Data.Entities
{
    public class Company
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]

        public Guid Id { get; set; }

        public string Name { get; set; }

    }
}
