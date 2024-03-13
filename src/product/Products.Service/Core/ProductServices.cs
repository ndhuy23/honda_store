using AutoMapper;
using MongoDB.Driver;
using ProductService.Data.DataAccess;
using ProductService.Data.Dto;
using ProductService.Data.Entities;
using ProductService.Data.Template;
using ProductService.Utils;

namespace Products.Services.Core
{
    public interface IProductService
    {
        public Task<ResultModel> Post(PostProductDto product);
        public ResultModel GetProductByCategory(GetProductByCategoryDto categoryDto);

        public Task<ResultModel> GetById(Guid id);
    }
    public class ProductServices : IProductService
    {
        readonly ProductDbContext _db;
        public ResultModel _result;
        readonly IMapper _mapper;

        public ProductServices(ProductDbContext db, IMapper mapper)
        {
            _db = db;
            _result = new ResultModel();
            _mapper = mapper;
        }
        public ResultModel GetProductByCategory(GetProductByCategoryDto categoryDto)
        {
            try
            {
                int offset = (categoryDto.Page - 1) * categoryDto.PageSize;
                Category category = _db.Category.Find(c => c.Name == categoryDto.CategoryName).First();
                _result.Data = _db.Product.Find(p => p.CategoryId == category.Id)
                                            .Skip(offset)
                                            .Limit(categoryDto.PageSize)
                                            .ToList();

                _result.IsSuccess = true;
            }
            catch (Exception e)
            {
                _result.IsSuccess = false;
                _result.Message = e.Message;
            }
            return _result;
        }
        public async Task<ResultModel> Post(PostProductDto product)
        {
            try
            {
                Product productNew = _mapper.Map<Product>(product);
                _db.Product.InsertOneAsync(productNew);
                for (int i = 0; i < productNew.ColorIds.Count(); i++)
                {
                    List<string> images = product.ColorImages[productNew.ColorIds[i]];
                    Guid colorId = productNew.ColorIds[i];
                    Storage storage = new Storage();
                    _db.Storage.InsertOneAsync(new Storage()
                    {
                        ColorId = productNew.ColorIds[i],
                        ProductId = productNew.Id,
                        Images = product.ColorImages[colorId],
                        Quantity = product.ColorQuantity[colorId]
                    });
                }

                _result.IsSuccess = true;
                _result.Message = "Post Product Successful";
            }
            catch (Exception e)
            {
                _result.IsSuccess = false;
                _result.Message = e.Message;
            }

            return _result;
        }
        public async Task<ResultModel> GetById(Guid id)
        {
            try
            {
                var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
                var product = _db.Product.AsQueryable().First(p => p.Id == id);
                _result.Data = product;
                _result.IsSuccess = true;
                _result.Message = "Get Product Successful";
            }
            catch (Exception e)
            {
                _result.IsSuccess = false;
                _result.Message = "Product is not exist";
            }
            return _result;
        }

    }
}
