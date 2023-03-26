using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCartAPI.Models
{
    [Keyless]
    public class Cart
    {
        public CartHeader CardHeader { get; set; }
        public IEnumerable<CartDetail> CartDetails { get; set; }
    }
}
