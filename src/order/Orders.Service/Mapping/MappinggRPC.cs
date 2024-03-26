using AutoMapper;
using Google.Protobuf.Collections;
using Orders.Data.Dto;
using Products.Service.GRPC.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Service.Mapping
{
    public class MappinggRPC : Profile
    {
        public MappinggRPC()
        {
            CreateMap<List<ProductDetail>, RepeatedField<ProductProtos>>()
                .ConstructUsing(src => MapListToRepeatedField(src)).ReverseMap();
        }
        private RepeatedField<ProductProtos> MapListToRepeatedField(List<ProductDetail> source)
        {
            var repeatedField = new RepeatedField<ProductProtos>();
            foreach (var productDetail in source)
            {
                var productProto = new ProductProtos
                {
                    ProductId = productDetail.ProductId.ToString(),
                    ColorId = productDetail.ColorId.ToString(),
                    Quantity = productDetail.Quantity
                };
                repeatedField.Add(productProto);
            }
            return repeatedField;
        }
    }
}
