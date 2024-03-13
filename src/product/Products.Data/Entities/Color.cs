using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProductService.Data.Entities
{
    public class Color
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        public string Image { get; set; }

        public string Name { get; set; }

    }
}
