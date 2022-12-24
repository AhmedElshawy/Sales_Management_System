using Core.Models;
using Core.ViewModels;

namespace Core.Interfaces
{
    public interface IProductRepository:IBaseRepository<Product>
    {
        Task<List<Product>> ApplayFillter(ProductParams productParams, int? numberOfItems);
        Task<List<Product>> ApplaySearchFillter(string search);
    }
}
