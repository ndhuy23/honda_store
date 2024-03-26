using AutoMapper;
using Google.Protobuf.Collections;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Orders.Data.DataAccess;
using Orders.Data.Dto;
using Orders.Data.Entities;
using Orders.Data.Response;
using Orders.Service.gRPC;
using Orders.Service.RabbitMQ.Producers;
using Products.Service.GRPC.Protos;
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
        readonly ProductMainService _productService;
        OrderDbContext _db;
        ResultModel _result;
        IMapper _mapper;
        OrderCreateProducer _orderCreateProducer;
        public OrderService(OrderDbContext db, ProductMainService productService, IMapper mapper, OrderCreateProducer order)
        {
            _productService = productService;
            _result = new ResultModel();
            _mapper = mapper;
            _db = db;
            _orderCreateProducer = order;
        }

        public async Task<ResultModel> CreateOrder(CreateOrderDto order)
        {
            using(var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    List<ProductDetail> listProductDetail = new List<ProductDetail>();
                    var listProductProtos = new RepeatedField<ProductProtos>();
                    order.Products.ForEach(product =>
                    {
                        var productProto = new ProductProtos
                        {
                            ProductId = product.ProductId.ToString(),
                            ColorId = product.ColorId.ToString(),
                            Quantity = product.Quantity
                        };
                        listProductProtos.Add(productProto);
                    });
                    var checkQuantityProductRequest = new CheckQuantityProductRequest
                    {
                        ListProduct = { listProductProtos }
                    };
                    var response = await _productService._client.CheckQuantityProductAsync(checkQuantityProductRequest);
                    if (!response.IsCheck)
                    {
                        _result.Data = response;
                        _result.IsSuccess = false;
                        _result.Message = "Product quantity is not enough";
                    }
                    else
                    {

                        long total = 0;
                        Order orderNew = new Order()
                        {
                            UserId = order.CustomerId,
                            Status = Status.LoadPayment,
                            IsPayment = false,
                            Total = 0
                        };

                        _db.Orders.Add(orderNew);
                        foreach (var product in order.Products)
                        {
                            OrderDetail orderDetail = new OrderDetail()
                            {
                                ColorId = product.ColorId,
                                OrderId = orderNew.Id,
                                ProductId = product.ProductId,
                                
                            };
                            total += product.Price;
                            listProductDetail.Add(product);
                            _db.OrderDetails.Add(orderDetail);
                        }
                        orderNew.Total = total;
                        _db.SaveChanges();
                        _result.IsSuccess = true;
                        _result.Message = "Order created";
                        transaction.Commit();
                    }

                    _orderCreateProducer.SendEvent(new RabbitMQ.Messages.OrderCreate
                    {
                        Products = listProductDetail
                    });
                    return _result;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    _result.IsSuccess = false;
                    _result.Message = e.Message;
                }
            }
            
            return _result;
        }
    }
}
