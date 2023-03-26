using AutoMapper;
using Mango.Services.ShoppingCartAPI.DbContexts;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCartAPI.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;
        private IMapper _mapper;

        public CartRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> ApplyCoupon(string userId, string couponId)
        {
            var cartFromDB = await _context.CartHeader.FirstOrDefaultAsync(x => x.UserId == userId);
            cartFromDB.CouponCode = couponId;
            _context.CartHeader.Update(cartFromDB);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ClearCart(string userId)
        {
            var cartHeaderFromDb = await _context.CartHeader.FirstOrDefaultAsync(u => u.UserId == userId);
            if(cartHeaderFromDb != null)
            {
                _context.CartDetails.RemoveRange(_context.CartDetails.Where(u => u.CartHeaderId == cartHeaderFromDb.CartHeaderId));
                _context.CartHeader.Remove(cartHeaderFromDb);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<CartDto> CreateUpdateCart(CartDto cartDto)
        {
            Cart cart = _mapper.Map<Cart>(cartDto);
            //check if product is available in DB
           var prodInDb = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == cartDto.CartDetails.FirstOrDefault().ProductId);

            if(prodInDb == null)
            {
                _context.Products.Add(cart.CartDetails.FirstOrDefault().Product);
                await _context.SaveChangesAsync();
            }

            //check if header is null
            var cartHeaderFromDb = await _context.CartHeader.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == cart.CardHeader.UserId);

            if(cartHeaderFromDb == null)
            {
                //create cartHeader & details
                _context.CartHeader.Add(cart.CardHeader);
                await _context.SaveChangesAsync();

                cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CardHeader.CartHeaderId;
                cart.CartDetails.FirstOrDefault().Product = null; //because the same product has already been added.
                _context.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                await _context.SaveChangesAsync();
            }

            //if header is not null
            else
            {
                //check if details has same product
                var cartDetailsFromDb = await _context.CartDetails.AsNoTracking().FirstOrDefaultAsync(u => u.ProductId == cart.CartDetails.FirstOrDefault().ProductId && u.CartHeaderId == cartHeaderFromDb.CartHeaderId);
                if( cartDetailsFromDb == null)
                {
                    //create details
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                    cart.CartDetails.FirstOrDefault().Product = null; //because the same product has already been added.
                    _context.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                    await _context.SaveChangesAsync();
                }
                else
                {
                    //update the count or cart details
                    cart.CartDetails.FirstOrDefault().Product = null; //because the same product has already been added.
                    cart.CartDetails.FirstOrDefault().Count +=  cartDetailsFromDb.Count;
                    _context.CartDetails.Update(cart.CartDetails.FirstOrDefault());
                    await _context.SaveChangesAsync();
                }
            }
            return _mapper.Map<CartDto>(cart);
        }

        public async Task<CartDto> GetCartByUserId(string userId)
        {
            Cart cart = new Cart
            {
                CardHeader = await _context.CartHeader.FirstOrDefaultAsync(u => u.UserId == userId)
            };
            cart.CartDetails = _context.CartDetails.Where(u => u.CartHeader.UserId == userId).Include(p => p.Product);
            return _mapper.Map<CartDto>(cart);
        }

        public async Task<bool> RemoveCoupon(string userId)
        {
            var cartFromDB = await _context.CartHeader.FirstOrDefaultAsync(x => x.UserId == userId);
            cartFromDB.CouponCode = "";
            _context.CartHeader.Update(cartFromDB);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveFromCart(int cardDetailsId)
        {
            try
            {
                CartDetail cart = await _context.CartDetails.FirstOrDefaultAsync(u => u.CartDetailsId == cardDetailsId);
                int totalCountOfCartItems = _context.CartDetails.Where(x => x.CartHeaderId == cart.CartHeaderId).Count();
                _context.CartDetails.Remove(cart);

                if (totalCountOfCartItems == 1)
                {
                    var cartToRemove = await _context.CartHeader.FirstOrDefaultAsync(u => u.CartHeaderId == cart.CartHeaderId);
                    _context.CartHeader.Remove(cartToRemove);                    
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
