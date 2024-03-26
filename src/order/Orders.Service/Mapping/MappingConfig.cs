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
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMap()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductProtos, ProductDetail>().ReverseMap();
                config.AddProfile<MappinggRPC>();
            });
            return mapperConfig;
        }
        
        
    }
}
