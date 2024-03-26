using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProductService.Data.Entities;
using ProductService.Data.Template;

namespace ProductService.Data.Entities

{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]

        public Guid Id { get; set; }
        
        public string Name { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Guid CategoryId { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Guid CompanyId {  get; set; }

        public List<Guid> ColorIds { get; set; } = new List<Guid>();
        
        public long Price { get; set; }

        public string Avatar { get; set; }

        public List<Feature> Features { get; set; } = new List<Feature>();

        public List<Detail> Details { get; set; } = new List<Detail>();

        public List<Preferential> Preferentials { get; set; } = new List<Preferential>();

        public List<Preferential> ExtendPreferentials { get; set; } = new List<Preferential>();

        
    }
}
