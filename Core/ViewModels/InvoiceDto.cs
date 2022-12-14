using Core.Constants;

namespace Core.ViewModels
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public decimal Amount { get; set; }
        public InvoiceStatus Status { get; set; }
        public CustomerDto Customer { get; set; }
        public List<InvoiceItemDto> InvoiceItems { get; set; }
    }
}
