using Mango.Services.OrderAPI.Models;
using Mango.Services.ShoppingCartAPI.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.OrderAPI.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContext;

        public OrderRepository(DbContextOptions<ApplicationDbContext> db)
        {
            _dbContext = db;
        }

        public async Task<bool> AddOrder(OrderHeader orderHeader)
        {
            await using var _db = new ApplicationDbContext(_dbContext);
            _db.OrderHeaders.Add(orderHeader);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task UpdateOrderPaymentStatus(int orderHeaderId, bool paid)
        {
            await using var _db = new ApplicationDbContext(_dbContext);
            var orderHeaderFromDb = await _db.OrderHeaders.FirstOrDefaultAsync(u => u.OrderHeaderId == orderHeaderId);
            if(orderHeaderFromDb != null)
            {
                orderHeaderFromDb.paymentStatus = paid;
                await _db.SaveChangesAsync();
            }

           // var querty = _db.Set<OrderHeader>().AsEnumerable().Join(_db.Set<OrderDetail>().AsEnumerable(),header => header.fie  )
        }
    }
}   
