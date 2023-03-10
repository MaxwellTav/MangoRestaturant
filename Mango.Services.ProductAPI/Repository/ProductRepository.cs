using AutoMapper;
using Mango.Services.ProductAPI.DbContexts;
using Mango.Services.ProductAPI.Models.Dto;
using Mango.Services.ProductAPI.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        readonly ApplicationDbContext _db;
        IMapper _mapper;

        public ProductRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<ProductDto> CreateUpdateProduct(ProductDto ProductDto)
        {
            Product product = _mapper.Map<ProductDto, Product>(ProductDto);

            //Updating the product.
            if (product.ProductId > 0)
                _db.Products.Update(product);
            //Adding a product.
            else    
                _db.Products.Add(product);

            await _db.SaveChangesAsync();
            return _mapper.Map<Product, ProductDto>(product);

        }

        public async Task<bool> DeleteProduct(int ProductId)
        {
            try
            {
                Product product = await _db.Products.FirstOrDefaultAsync(p => p.ProductId == ProductId);

                if (product == null)
                    return false;

                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<ProductDto> GetProductById(int ProductId)
        {
            Product product = await _db.Products.Where(p => p.ProductId == ProductId).FirstOrDefaultAsync();
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            List<Product> productsList = await _db.Products.ToListAsync();
            return _mapper.Map<List<ProductDto>>(productsList);
        }
    }
}
