﻿using Mango.Services.ShoppingCartAPI.Models.Dto;

namespace Mango.Services.ShoppingCartAPI.Repository
{
    public interface ICartRepository
    {
        Task<CartDto> GetCartByUserId(string userId);
        Task<CartDto> CreateUpdateCart(CartDto cartDto);
        Task<bool> RemoveFromCart(int cardDetailsId);
        Task<bool> ClearCart(string userId);
        Task<bool> ApplyCoupon(string userId, string couponId);
        Task<bool> RemoveCoupon(string userId);
    }
}
