using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using Products.Data.Dto;
using Products.Service.GRPC.Protos;
using Products.Services.Core;
namespace Products.Service.gRPC.Services
{
    
    public class ProductMainServicegRPC : ProductServicegRPC.ProductServicegRPCBase 
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductMainServicegRPC(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        public async override Task<CheckQuantityProductResponse> CheckQuantityProduct
            (CheckQuantityProductRequest request, ServerCallContext context)
        {
            var products = _mapper.Map<List<ProductProtos>,List<ProductDetail>>(request.ListProduct.ToList());
            var isSuccess = await _productService.CheckProductQuantityAsync(products);
            return new CheckQuantityProductResponse() { IsCheck = isSuccess };
        }
    }
}
