using Core.Interfaces;
using Core.Models;
using Infrastructure.Context;
using Infrastructure.Repositories;

namespace Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IBaseRepository<Product> Products { get; private set; }

        public IBaseRepository<Category> Categories { get; private set; }

        public IBaseRepository<Brand> Brands { get; private set; }

        public IBaseRepository<Invoice> Invoices { get; private set; }

        public IBaseRepository<InvoiceItem> InvoiceItems { get; private set; }
        public IBaseRepository<Customer> Customers { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context= context;
            Products = new BaseRepository<Product>(context);
            Categories = new BaseRepository<Category>(context);
            Brands= new BaseRepository<Brand>(context);
            Invoices = new BaseRepository<Invoice>(context);
            InvoiceItems= new BaseRepository<InvoiceItem>(context);
            Customers = new BaseRepository<Customer>(context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
