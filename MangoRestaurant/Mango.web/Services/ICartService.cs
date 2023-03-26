using Mango.web.Models;

namespace Mango.web.Services
{
    public interface ICartService
    {
        Task<T> GetCartByUserIdAsync<T>(string userId, string token=null);
        Task<T> AddToCartByUserIdAsync<T>(CartDto cartDto, string token=null);
        Task<T> UpdateCartByUserIdAsync<T>(CartDto cartDto, string token=null);
        Task<T> RemoveCartByUserIdAsync<T>(int cartId, string token=null);
        Task<T> ApplyCoupon<T>(CartDto cartDto, string token=null);
        Task<T> RemoveCoupon<T>(string userId, string token=null);
        Task<T> Checkout<T>(CartHeaderDto cartHeaderDto, string token=null);
    }
}
