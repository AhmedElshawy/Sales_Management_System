using Core.Interfaces;
using Core.Models;
using Core.ViewModels;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context):base(context)
        {
            _context= context;
        }      

        public async Task<List<Product>> ApplayFillter(ProductParams productParams)
        {
            IQueryable<Product> query = _context.Products;

            if (productParams.BrandId != null)
            {
                query = query.Where(m=>m.BrandId== productParams.BrandId);
            }
            if (productParams.CategoryId != null)
            {
                query = query.Where(m => m.CategoryId == productParams.CategoryId);
            }
            if (productParams.Search != null)
            {
                query = query.Where(m=>m.Name.ToLower().Contains(productParams.Search));
            }

            return await query.ToListAsync();
        }

        public async Task<List<Product>> ApplaySearchFillter(string search)
        {
            IQueryable<Product> query = _context.Products;
            if(!string.IsNullOrEmpty(search))
            {
                query = query.Where(m => m.Name.ToLower().Contains(search));
            }
            return await query.ToListAsync();
        }
    }
}
