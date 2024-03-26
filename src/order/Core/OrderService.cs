using Grpc.Net.Client;
using Orders.Data.Dto;
using Orders.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Service.Core
{
    public interface IOrderService
    {
        Task<ResultModel> CreateOrder(CreateOrderDto order);
    }
    public class OrderService : IOrderService
    {
        private readonly GrpcChannel _channel;
        private readonly Productervice.ProductOfferServiceClient _client;
        private readonly IConfiguration _configuration;


        public Task<ResultModel> CreateOrder(CreateOrderDto order)
        {
            
        }
    }
}
