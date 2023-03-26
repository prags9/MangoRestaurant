using AutoMapper;
using Mango.services.ProductApi.DbContexts;
using Mango.services.ProductApi.Models;
using Mango.services.ProductApi.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Mango.services.ProductApi.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private IMapper _mapper;

        public ProductRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProductDto> CreateUpdateproduct(ProductDto productDto)
        {
            Product prod = _mapper.Map<ProductDto, Product>(productDto);
            if(prod.ProductId > 0)
            {
                _context.products.Update(prod);
            }
            else
            {
                _context.Add(prod);
            }
            await _context.SaveChangesAsync();
            return _mapper.Map<Product, ProductDto>(prod);
        }

        public async Task<bool> Deleteproduct(int id)
        {
            try
            {
                Product prod = await _context.products.FirstOrDefaultAsync(x => x.ProductId == id);
                if(prod == null)
                {
                    return false;
                }
                _context.Remove(prod);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<ProductDto> GetProductById(int id)
        {
            Product prod = await _context.products.Where(x => x.ProductId == id).FirstOrDefaultAsync();
            return _mapper.Map<ProductDto>(prod);
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            IEnumerable<Product> list = await _context.products.ToListAsync();
            return _mapper.Map<List<ProductDto>>(list);
        }
    }
}
