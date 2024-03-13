using ProductService.Data.DataAccess;
using ProductService.Data.Dto;
using ProductService.Data.Entities;

namespace Products.Services.Core
{
    public interface IPreferentialService
    {
        public ResultModel Post(string content);
    }
    public class PreferentialService : IPreferentialService
    {
        readonly ProductDbContext _db;
        public ResultModel _result;

        public PreferentialService(ProductDbContext db)
        {
            _db = db;
            _result = new ResultModel();
        }
        public ResultModel Post(string content)
        {
            try
            {

                _db.Preferential.InsertOne(new Preferential() { Content = content });

                _result.IsSuccess = true;

                _result.Message = "Post Color Successful";
            }
            catch (Exception e)
            {
                _result.IsSuccess = false;
                _result.Message = e.Message;
            }
            return _result;

        }
    }
}
