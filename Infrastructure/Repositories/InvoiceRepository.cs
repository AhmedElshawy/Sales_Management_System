using cloudscribe.Pagination.Models;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class InvoiceRepository : BaseRepository<Invoice>, IInvoiceRepository
    {
        private readonly AppDbContext _context;
        public InvoiceRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagedResult<Invoice>> ApplayFilterWithPagination(DateTime? from , DateTime? to, int pageNumber,int pageSize)
        {
            var query =  _context.Invoices.AsQueryable();
            if (from != null && to != null)
            {
                query = query.Where(d => d.Date >= from.Value.Date && d.Date <= to.Value.Date);
            }

            return await ApplayPagination(pageNumber,pageSize,query);
        }

        public async Task<Invoice> GetInvoiceWihtDeatils(int id)
        {
            var invoice = await _context.Invoices
                .Include(i=>i.Customer)
                .Include(i=>i.InvoiceItems)
                .FirstOrDefaultAsync(e=>e.Id == id);

            return invoice;
        }

        private async Task<PagedResult<Invoice>> ApplayPagination(int pageNumber, int pageSize, IQueryable<Invoice> query)
        {
            int excludedRecords = (pageNumber * pageSize) - pageSize;
            int totalItems = await query.CountAsync();
            query = query.Skip(excludedRecords).Take(pageSize);

            var result = new PagedResult<Invoice>();
            result.Data = await query.Include(i=>i.Customer).ToListAsync();
            result.TotalItems = totalItems;
            result.PageNumber = pageNumber;
            result.PageSize = pageSize;

            return result;
        }
    }
}
