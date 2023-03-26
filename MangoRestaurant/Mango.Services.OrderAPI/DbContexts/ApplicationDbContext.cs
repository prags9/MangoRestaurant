using Mango.Services.OrderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCartAPI.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> Orderdetails { get; set; }

    }
}
