using Mango.web.Models;
using Mango.web.Services.IServices;

namespace Mango.web.Services
{
    public class ProductService : BaseService, IProductService
    {
        private IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T> CreateProductAsync<T>(ProductDto productDto, string token)
        {
           return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = productDto,
                Url = SD.ProductAPIBase + "/api/products",
                AccessToken = token
            });
        }

        public async Task<T> DeleteProductAsync<T>(int id, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.ProductAPIBase + "/api/products/"+id,
                AccessToken = token
            });
        }

        public async Task<T> GetAllProductsAsync<T>(string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ProductAPIBase + "/api/products",
                AccessToken = token
            });
        }

        public async Task<T> GetProductByIdAsync<T>(int id, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ProductAPIBase + "/api/products/" + id,
                AccessToken = token
            });
        }

        public async Task<T> UpdateProductAsync<T>(ProductDto productDto, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = productDto,
                Url = SD.ProductAPIBase + "/api/products",
                AccessToken = token
            });
        }
    }
}
