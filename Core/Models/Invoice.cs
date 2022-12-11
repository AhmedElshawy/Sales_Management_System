using Core.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;
        public decimal Amount { get; set; }
        public InvoiceStatus Status { get; set; } = InvoiceStatus.Pending;

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        //navigation properities
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
