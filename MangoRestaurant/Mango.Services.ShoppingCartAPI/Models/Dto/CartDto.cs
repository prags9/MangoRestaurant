using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCartAPI.Models.Dto
{
    
    public class CartDto
    {
        public CartHeader CardHeader { get; set; }
        public IEnumerable<CartDetailDto> CartDetails { get; set; }
    }
}
