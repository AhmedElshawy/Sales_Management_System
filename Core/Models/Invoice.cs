using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public DateTimeOffset InvoiceDate { get; set; } = DateTimeOffset.Now;
        public decimal Amount { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        //navigation properities
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
