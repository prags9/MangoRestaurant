using Mango.services.ProductApi.Models.Dto;

namespace Mango.services.ProductApi.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDto>> GetProducts();
        Task<ProductDto> GetProductById(int id);
        Task<ProductDto> CreateUpdateproduct(ProductDto productDto);
        Task<bool> Deleteproduct(int id);
    }
}
