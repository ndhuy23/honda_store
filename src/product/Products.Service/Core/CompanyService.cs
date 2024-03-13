using ProductService.Data.DataAccess;
using ProductService.Data.Dto;
using ProductService.Data.Entities;

namespace Products.Services.Core
{
    public interface ICompanyService
    {
        public ResultModel Post(string name);
    }
    public class CompanyService : ICompanyService
    {
        readonly ProductDbContext _db;
        public ResultModel _result;

        public CompanyService(ProductDbContext db)
        {
            _db = db;
            _result = new ResultModel();
        }
        public ResultModel Post(string name)
        {
            try
            {

                _db.Company.InsertOne(new Company() { Name = name });

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
