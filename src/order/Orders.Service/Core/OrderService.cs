using AutoMapper;
using Google.Protobuf.Collections;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Orders.Data.DataAccess;
using Orders.Data.Dto;
using Orders.Data.Dto.Response;
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
        Task<ResultModel> ChangeStatusFromPayment(Guid orderId);
        Task<ResultModel> CreateOrder(CreateOrderDto order);
        Task<ResultModel> GetByUserId(Guid userId); 
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


        public async Task<ResultModel> ChangeStatusFromPayment(Guid orderId)
        {
            using(var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var order = _db.Orders.Where(od => od.Id == orderId).FirstOrDefault();
                    order.Status = Status.LoadAccept;
                    _result.IsSuccess = true;
                    _db.SaveChanges();
                    transaction.Commit();
                }catch(Exception e)
                {
                    transaction.Rollback();
                    _result.IsSuccess = false;
                    _result.Message = e.Message;
                }

            }
            return _result;
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
                            Total = 0,
                            OrderDetails = new List<OrderDetail>()
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
                            orderNew.OrderDetails.Add(orderDetail);
                            total += product.Price;
                            listProductDetail.Add(product);
                            _db.OrderDetails.Add(orderDetail);
                        }
                        orderNew.Total = total;
                        _db.SaveChanges();
                        _result.Data = new OrderResponse() { 
                            UserId = order.CustomerId,
                            OrderId = orderNew.Id,
                            Amount = total
                        };
                        _result.IsSuccess = true;
                        _result.Message = "Order created";
                        transaction.Commit();
                    }

                    _orderCreateProducer.SendEvent(new RabbitMQ.Messages.OrderCreate
                    {
                        Products = listProductDetail
                    });
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

        public async Task<ResultModel> GetByUserId(Guid userId)
        {
            try
            {
                var listOrder = _db.Orders.Where(od => od.UserId == userId).ToList();
                
                listOrder.ForEach(order =>
                {
                    var orderDetail = _db.OrderDetails.Where(od => od.OrderId == order.Id)
                                                      .Select(od => new OrderDetail { ColorId = od.ColorId,
                                                                                      ProductId = od.ProductId})
                                                      .ToList();
                    order.OrderDetails = orderDetail;
                });
                _result.Data = listOrder;
                _result.IsSuccess = true;
                _result.Message = "";
            }
            catch(Exception e)
            {
                _result.IsSuccess = false;
                _result.Message = e.Message;
            }
            return _result;
        }
    }
}
