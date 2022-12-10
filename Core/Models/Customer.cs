
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        //navigation props
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
