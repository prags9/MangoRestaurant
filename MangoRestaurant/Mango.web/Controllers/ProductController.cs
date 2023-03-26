using Mango.web.Models;
using Mango.web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto> list = new();
            var acessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _productService.GetAllProductsAsync<ResponseDto>(acessToken);
            if(response is not null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductCreate(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                var acessToken = await HttpContext.GetTokenAsync("access_token");
                var response = await _productService.CreateProductAsync<ResponseDto>(productDto, acessToken);
                if (response is not null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }
            return View(productDto);
        }
        public async Task<IActionResult> ProductEdit(int productid)
        {
            var acessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _productService.GetProductByIdAsync<ResponseDto>(productid, acessToken);
            if (response is not null && response.IsSuccess)
            {
                ProductDto model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                return View(model);
            }
            
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductEdit(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                var acessToken = await HttpContext.GetTokenAsync("access_token");
                var response = await _productService.UpdateProductAsync<ResponseDto>(productDto, acessToken);
                if (response is not null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }
            return View(productDto);
        }
        public async Task<IActionResult> ProductDelete(int productid)
        {
            var acessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _productService.GetProductByIdAsync<ResponseDto>(productid, acessToken);
            if (response is not null && response.IsSuccess)
            {
                ProductDto model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                return View(model);
            }

            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductDelete(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                var acessToken = await HttpContext.GetTokenAsync("access_token");
                var response = await _productService.DeleteProductAsync<ResponseDto>(productDto.ProductId, acessToken);
                if (response is not null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }
            return View(productDto);
        }
    }
}
