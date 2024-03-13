using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProductService.Data.Entities;

namespace ProductService.Data.DataAccess
{
    public class ProductDbContext
    {
        IMongoClient _client;
        IMongoDatabase _database;

        public ProductDbContext(IOptions<ProductAppDbSetting> OrderAppDatabaseSetting)
        {
            _client = new MongoClient(OrderAppDatabaseSetting.Value.ConnectionString);
            _database = _client.GetDatabase(OrderAppDatabaseSetting.Value.DatabaseName);
            var storageCollection = _database.GetCollection<Storage>("storages");
            var key = Builders<Storage>.IndexKeys
                .Text(s => s.ProductId);
            var indexModel = new CreateIndexModel<Storage>(key);
            storageCollection.Indexes.CreateOne(key);

        }

        public IMongoCollection<Category> Category => _database.GetCollection<Category>("categories");

        public IMongoCollection<Color> Color => _database.GetCollection<Color>("colors");

        public IMongoCollection<Preferential> Preferential => _database.GetCollection<Preferential>("preferentials");

        public IMongoCollection<Product> Product => _database.GetCollection<Product>("products");

        public IMongoCollection<Storage> Storage => _database.GetCollection<Storage>("storages");

        public IMongoCollection<Company> Company => _database.GetCollection<Company>("companies");


        public IClientSessionHandle StartSession()
        {
            var session = _client.StartSession();
            return session;
        }

        public void Create()
        {
            var CollectionNames = _database.ListCollectionNames().ToList();
            if (!CollectionNames.Any(name => name.Equals("categories")))
            {
                _database.CreateCollection("categories");
            }
            if (!CollectionNames.Any(name => name.Equals("colors")))
            {
                _database.CreateCollection("colors");
            }
            if (!CollectionNames.Any(name => name.Equals("preferentials")))
            {
                _database.CreateCollection("preferentials");
            }
            if (!CollectionNames.Any(name => name.Equals("products")))
            {
                _database.CreateCollection("products");
            }
            if (!CollectionNames.Any(name => name.Equals("storages")))
            {
                _database.CreateCollection("storages");
            }
            if (!CollectionNames.Any(name => name.Equals("companies")))
            {
                _database.CreateCollection("companies");
            }
            
        }
    }
}
