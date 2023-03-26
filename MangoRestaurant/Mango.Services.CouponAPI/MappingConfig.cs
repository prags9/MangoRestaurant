using AutoMapper;
using Mango.services.CouponAPI.Models.Dto;
using Mango.Services.CouponAPI.Models;

namespace Mango.Services.CouponAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {                
                config.CreateMap<CouponDto, Coupon>().ReverseMap();
            });
            return mapperConfig;
        }
    }
}
