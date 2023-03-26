using Mango.web.Models;
using Mango.web.Services;
using Mango.web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.web.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly ICouponService _couponService;

        public CartController(IProductService productService, ICartService cartService, ICouponService couponService)
        {
            _productService = productService;
            _cartService = cartService;
            _couponService = couponService;
        }

        public async Task<IActionResult> CartIndex()
        { 

            return View(await LoadCartBasedOnLoggedInUser());
        }

        [HttpPost]
        [ActionName("ApplyCoupon")]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {

            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var resp = await _cartService.ApplyCoupon<ResponseDto>(cartDto, accessToken);
            if (resp != null && resp.IsSuccess)
            {
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        [HttpPost]
        [ActionName("RemoveCoupon")]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {

            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var resp = await _cartService.RemoveCoupon<ResponseDto>(cartDto.CardHeader.UserId, accessToken);
            if (resp != null && resp.IsSuccess)
            {
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var resp = await _cartService.RemoveCartByUserIdAsync<ResponseDto>(cartDetailsId, accessToken);
            if (resp != null && resp.IsSuccess)
            {
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }
        
        public async Task<IActionResult> Checkout()
        {

            return View(await LoadCartBasedOnLoggedInUser());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CartDto cartDto)
        {

            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var resp = _cartService.Checkout<ResponseDto>(cartDto.CardHeader, accessToken);
                return RedirectToAction(nameof(Confirmation));
            }
            catch(Exception e)
            {
                return View(cartDto);
            }
        }

       
        public async Task<IActionResult> Confirmation()
        {
            return View();
        }

        private async Task<CartDto> LoadCartBasedOnLoggedInUser()
        {
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var resp = await _cartService.GetCartByUserIdAsync<ResponseDto>(userId, accessToken);
            CartDto cartDto = new();
            if(resp != null && resp.IsSuccess)
            {
                cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(resp.Result));
            }

            if(cartDto.CardHeader != null)
            {
                if (!string.IsNullOrEmpty(cartDto.CardHeader.CouponCode))
                {
                    var coupon = await _couponService.GetCoupon<ResponseDto>(cartDto.CardHeader.CouponCode, accessToken);
                    if (coupon != null && coupon.IsSuccess)
                    {
                        var couponobj = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(coupon.Result));
                        cartDto.CardHeader.DiscountTotal = couponobj.DiscountAmount;
                    }
                }
                foreach (var detail in cartDto.CartDetails)
                {
                    cartDto.CardHeader.OrderTotal += (detail.Product.Price * detail.Count);
                }
                cartDto.CardHeader.OrderTotal -=cartDto.CardHeader.DiscountTotal; 
            }
            return cartDto;
        }
    }
}
