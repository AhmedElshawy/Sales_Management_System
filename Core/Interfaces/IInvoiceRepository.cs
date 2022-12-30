using Core.Models;
using cloudscribe.Pagination.Models;

namespace Core.Interfaces
{
    public interface IInvoiceRepository: IBaseRepository<Invoice>
    {
        Task<PagedResult<Invoice>> ApplayFilterWithPagination(DateTime? from, DateTime? to, int pageNumber, int pageSize);
        Task<Invoice> GetInvoiceWihtDeatils(int id);
    }
}
