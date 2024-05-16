using AutoMapper;
using MassTransit.Internals.Caching;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using Products.Data.Dto;
using Products.Service.GRPC.Protos;
using ProductService.Data.DataAccess;
using ProductService.Data.Dto;
using ProductService.Data.Entities;
using ProductService.Data.Template;
using ProductService.Utils;
using System.Collections;

namespace Products.Services.Core
{
    public interface IProductService
    {
        public Task<ResultModel> Post(PostProductDto product);
        public ResultModel GetProductByCategory(GetProductByCategoryDto categoryDto);

        public Task<ResultModel> GetById(Guid id);
        Task<bool> CheckProductQuantityAsync(List<ProductDetail> products);

        public Task<ResultModel> Get(int pageIndex, int pageSize);
    }
    public class ProductServices : IProductService
    {
        private readonly IDistributedCache _cache;
        readonly ProductDbContext _db;
        public ResultModel _result;
        readonly IMapper _mapper;

        public ProductServices(ProductDbContext db, IMapper mapper, IDistributedCache cache)
        {
            _db = db;
            _result = new ResultModel();
            _mapper = mapper;
            _cache = cache;
        }
        public ResultModel GetProductByCategory(GetProductByCategoryDto categoryDto)
        {
            try
            {
                string cachedData = _cache.GetString($"{categoryDto.CategoryName}_{categoryDto.Page}_{categoryDto.PageSize}");
                if (string.IsNullOrEmpty(cachedData))
                {
                    var cacheOptions = new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) // Thời gian sống của cache là 5 phút
                    };

                    int offset = (categoryDto.Page - 1) * categoryDto.PageSize;
                    Category category = _db.Category.Find(c => c.Name == categoryDto.CategoryName).First();
                    _result.Data = _db.Product.Find(p => p.CategoryId == category.Id)
                                                .Skip(offset)
                                                .Limit(categoryDto.PageSize)
                                                .ToList();
                    _cache.SetString(categoryDto.CategoryName, JsonConvert.SerializeObject(_result.Data), cacheOptions);
                }
                else
                {
                    _result.Data = JsonConvert.DeserializeObject<List<Product>>(cachedData);
                }
                

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
                var minPrice = product.ProductTypes[1].Price;
                for (int i = 0; i < product.ProductTypes.Count(); i++)
                {
                    productNew.ColorIds.Add(product.ProductTypes[i].Color);
                    Storage storage = new Storage();
                    _db.Storage.InsertOneAsync(new Storage()
                    {
                        ColorId = product.ProductTypes[i].Color,
                        ProductId = productNew.Id,
                        Images = product.ProductTypes[i].Images,
                        Quantity = product.ProductTypes[i].Quantity,
                        Price = product.ProductTypes[i].Price
                    });
                    if (product.ProductTypes[i].Price < minPrice) minPrice = product.ProductTypes[i].Price;
                }
                var update = Builders<Product>.Update.Set(p => p.ColorIds, productNew.ColorIds)
                                                     .Set(p => p.Price, minPrice);
                var filter = Builders<Product>.Filter.Eq(p => p.Id, productNew.Id);
                
                _db.Product.UpdateOne(filter, update);
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
                string cachedData = _cache.GetString($"Product_{id}");
                if (string.IsNullOrEmpty(cachedData))
                {
                    var cacheOptions = new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) // Thời gian sống của cache là 5 phút
                    };
                    var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
                    var product = _db.Product.AsQueryable().First(p => p.Id == id);
                    _result.Data = product;
                    _cache.SetString($"Product_{id}", JsonConvert.SerializeObject(_result.Data), cacheOptions);

                }
                
                else
                {
                    _result.Data = JsonConvert.DeserializeObject<Product>(cachedData);
                }
                    
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
        public async Task<bool> CheckProductQuantityAsync(List<ProductDetail> products)
        {
            for (int i = 0; i < products.Count; i++)
            {
                var productStorage = await _db.Storage.AsQueryable()
                                                        .Where(p => p.ProductId == products[i].ProductId)
                                                        .FirstOrDefaultAsync();
                if (productStorage == null ||
                    productStorage.Quantity < products[i].Quantity) return false;
            }
            
            return true;
        }

        public async Task<ResultModel> Get(int pageIndex, int pageSize)
        {
            try
            {
                string cachedData = _cache.GetString($"Product_{pageIndex}_{pageSize}");
                if (string.IsNullOrEmpty(cachedData))
                {
                    var cacheOptions = new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) // Thời gian sống của cache là 5 phút
                    };
                    _result.Data = _db.Product.AsQueryable().Skip((pageIndex - 1) * pageSize)
                                                        .Take(pageSize);
                    _cache.SetString($"Product_{pageIndex}_{pageSize}", JsonConvert.SerializeObject(_result.Data), cacheOptions);

                }
                else
                {
                    _result.Data = JsonConvert.DeserializeObject<List<Product>>(cachedData);
                }

                _result.IsSuccess = true;
                _result.Message = "";
            }catch(Exception e)
            {
                _result.IsSuccess = false;
                _result.Message = "";
            }
            return _result;
        }
    }
}
