using MongoDB.Bson;
using MongoDB.Driver;
using ProductService.Data.DataAccess;
using ProductService.Data.Dto;
using ProductService.Data.Entities;

namespace Products.Services.Core
{
    public interface IStorageService
    {
        Task<ResultModel> GetByProductIdAndColorId(Guid ProductId, Guid ColorId);
        Task<ResultModel> GetByProductId(Guid id);
    }
    public class StorageService : IStorageService
    {

        readonly ProductDbContext _db;
        ResultModel _result;
        public StorageService(ProductDbContext db)
        {
            _db = db;
            _result = new ResultModel();
        }

        public async Task<ResultModel> GetByProductIdAndColorId(Guid ProductId, Guid ColorId)
        {
            try
            {
                var storage = _db.Storage.AsQueryable().First(s => s.ProductId == ProductId
                                                        && s.ColorId == ColorId);
                _result.Data = storage;
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
                var filter = Builders<Storage>.Filter.Eq("ProductId", id);
                var resultCursor = await _db.Storage.FindAsync<Storage>(filter);
                _result.Data = await resultCursor.ToListAsync();
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
