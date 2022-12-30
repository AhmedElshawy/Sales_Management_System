using Core.Models;

namespace Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Product> Products { get; }
        IBaseRepository<Category> Categories { get; }
        IBaseRepository<Brand> Brands { get; }
        IBaseRepository<Invoice> Invoices { get; }
        IBaseRepository<InvoiceItem> InvoiceItems { get; }
        IBaseRepository<Customer> Customers { get; }
        IBaseRepository<AdditionOrder> AdditionOrders { get; }
        IProductRepository ProductRepository { get; }
        
        Task<int> CompleteAsync();
    }
}
