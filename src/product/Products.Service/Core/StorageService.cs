using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using ProductService.Data.DataAccess;
using ProductService.Data.Dto;
using ProductService.Data.Entities;
using System.Drawing;

namespace Products.Services.Core
{
    public interface IStorageService
    {
        Task<ResultModel> GetByProductIdAndColorId(Guid ProductId, Guid ColorId);
        Task<ResultModel> GetByProductId(Guid id);
    }
    public class StorageService : IStorageService
    {
        private readonly IDistributedCache _cache;
        readonly ProductDbContext _db;
        ResultModel _result;
        public StorageService(ProductDbContext db, IDistributedCache cache)
        {
            _db = db;
            _result = new ResultModel();
            _cache = cache;
        }

        public async Task<ResultModel> GetByProductIdAndColorId(Guid ProductId, Guid ColorId)
        {
            try
            {

                string cachedData = _cache.GetString($"{ProductId}_{ColorId}");
                if (string.IsNullOrEmpty(cachedData))
                {
                    var cacheOptions = new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) // Thời gian sống của cache là 5 phút
                    };
                    var storage = _db.Storage.AsQueryable().First(s => s.ProductId == ProductId
                                                        && s.ColorId == ColorId);
                    _result.Data = storage;
                    await _cache.SetStringAsync($"{ProductId}_{ColorId}", JsonConvert.SerializeObject(_result.Data), cacheOptions);
                }
                else
                {
                    _result.Data = JsonConvert.DeserializeObject<Storage>(cachedData);
                }
                _result.IsSuccess = true;
                _result.Message = "Get Succesful";

            }
            catch (Exception e)
            {
                _result.IsSuccess = false;
                _result.Message = "Product or color is not exist";
            }
            return _result;

        }

        public async Task<ResultModel> GetByProductId (Guid id)
        {
            try
            {
                string cachedData = _cache.GetString($"Storage_{id}");
                if (string.IsNullOrEmpty(cachedData))
                {
                    var cacheOptions = new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) // Thời gian sống của cache là 5 phút
                    };
                    var filter = Builders<Storage>.Filter.Eq("ProductId", id);
                    var resultCursor = await _db.Storage.FindAsync<Storage>(filter);
                    _result.Data = await resultCursor.ToListAsync();
                    await _cache.SetStringAsync($"Storage_{id}", JsonConvert.SerializeObject(_result.Data), cacheOptions);

                }
                else
                {
                    _result.Data = JsonConvert.DeserializeObject<List<Storage>>(cachedData);
                }

                _result.IsSuccess = true;
                _result.Message = "Get Successful";
            }catch (Exception e)
            {
                _result.IsSuccess = false;
                _result.Message = e.Message;
            }
            return _result;
        }
    }
}
