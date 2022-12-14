using System.ComponentModel.DataAnnotations.Schema;

namespace Core.ViewModels
{
    public class InvoiceItemDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal UnitPrice { get; set; }

    }
}
