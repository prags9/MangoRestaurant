using AutoMapper;
using Mango.services.CouponAPI.Models.Dto;
using Mango.Services.CouponAPI.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public CouponRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<CouponDto> GetCouponByCode(string code)
        {
            var couponFromDb = await _db.Coupons.FirstOrDefaultAsync(u => u.CouponCode == code);
            return _mapper.Map<CouponDto>(couponFromDb);
        }
    }
}
