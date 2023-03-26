using Mango.services.ProductApi.Models.Dto;
using Mango.services.ProductApi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.services.ProductApi.Controllers
{
    [Route("api/products")]
    public class ProductApiController : ControllerBase
    {
        protected ResponseDto _response;
        private IProductRepository productRepository;

        public ProductApiController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
            this._response = new ResponseDto();
        }
        //[Authorize]
        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                IEnumerable<ProductDto> productDtos = await productRepository.GetProducts();
                _response.Result = productDtos;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
            }
            return _response;
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<object> Get(int id)
        {
            try
            {
                ProductDto productDtos = await productRepository.GetProductById(id);
                _response.Result = productDtos;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
            }
            return _response;
        }
        [HttpPost]
        public async Task<object> Post([FromBody] ProductDto productDto)
        {
            try
            {
                ProductDto productDtos = await productRepository.CreateUpdateproduct(productDto);
                _response.Result = productDtos;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
            }
            return _response;
        }
        [HttpPut]
        public async Task<object> Put([FromBody] ProductDto productDto)
        {
            try
            {
                ProductDto productDtos = await productRepository.CreateUpdateproduct(productDto);
                _response.Result = productDtos;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
            }
            return _response;
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<object> Delete(int id)
        {
            try
            {
                bool isSuccess = await productRepository.Deleteproduct(id);
                _response.Result = isSuccess;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
            }
            return _response;
        }
    }
}
