using AutoMapper;
using Mango.services.ProductApi.Models;
using Mango.services.ProductApi.Models.Dto;

namespace Mango.services.ProductApi
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDto, Product>();
                config.CreateMap<Product, ProductDto>();
            });
            return mapperConfig;
        }
    }
}
