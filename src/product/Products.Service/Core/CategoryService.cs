
using ProductService.Data.DataAccess;
using ProductService.Data.Dto;
using ProductService.Data.Entities;

namespace Products.Services.Core
{
    public interface ICategoryService
    {
        public ResultModel Post(string categoryName);
    }
    public class CategoryService : ICategoryService
    {
        readonly ProductDbContext _db;
        public ResultModel _result;
        public CategoryService(ProductDbContext db)
        {
            _db = db;
            _result = new ResultModel();
        }

        public ResultModel Post(string categoryName)
        {
            try
            {
                _db.Category.InsertOne(new Category()
                {
                    Name = categoryName
                });
            }
            catch (Exception e)
            {
                _result.IsSuccess = false;
                _result.Message = e.Message;
            }

            _result.IsSuccess = true;
            _result.Message = "Post Category Successful";
            return _result;
        }
    }
}
