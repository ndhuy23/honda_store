using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Products.Service.GRPC.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Service.gRPC
{
    public class ProductMainService
    {
        private readonly GrpcChannel _channel;
        public readonly ProductServicegRPC.ProductServicegRPCClient _client;
        private readonly IConfiguration _configuration;

        public ProductMainService(IConfiguration configuration)
        {
            _configuration = configuration;
            _channel =
                GrpcChannel.ForAddress(_configuration.GetValue<string>("GrpcSettings:ProductMainServiceUrl"));
            _client = new ProductServicegRPC.ProductServicegRPCClient(_channel);
        }
    }
}
