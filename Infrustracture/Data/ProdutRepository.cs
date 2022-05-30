using API;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrustracture.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _context;
        public ProductRepository(StoreContext context)
        {
            this._context = context;
        }

        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        {
            return await _context.ProductBrands.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products
            .Include(p => p.ProductBrand)
            .Include(p => p.ProductType)
            .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            // var query = from s in _context.ProductPrices
            //             from p in _context.Products
            //             where (s.ProductId == p.Id && s.IsActive == true)
            //             select new { price = s.Price };
            // return await query.ToListAsync();
            return await _context.Products
             .Include(p => p.ProductBrand)
            .Include(p => p.ProductType)
            // .Join((_context.ProductPrices,
            // c => c.Id,
            // cm => cm.ProductId,
            // (c, cm) => new
            // {
            //     productId = c.Id,
            //     price = cm.Price,
            //     act = cm.IsActive,
            // }).where(p=>p.act == true).Select(p=>p.price))
            .ToListAsync();
        }

        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
        {
            return await _context.ProductTypes.ToListAsync();
        }
    }
}