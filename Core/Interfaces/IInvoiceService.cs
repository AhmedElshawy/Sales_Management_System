
using Core.Models;
using Core.ViewModels;

namespace Core.Interfaces
{
    public interface IInvoiceService
    {
        Task<Invoice> GenerateInvoiceAsync(List<InvoiceItemDto> invoiceItemsDto, CustomerDto customerDto);
    }
}
