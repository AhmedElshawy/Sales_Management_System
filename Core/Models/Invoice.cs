namespace Core.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public DateTimeOffset InvoiceDate { get; set; } = DateTimeOffset.Now;
        public decimal TotalPrice { get; set; }

        //navigation properities
        public ICollection<InvoiceItem> InvoiceItems { get; set; }
    }
}
