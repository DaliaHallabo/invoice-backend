using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Domain.Entities
{
    public class InvoiceApp
    {
        public int Id { get; set; }
        public string Serial { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        public int StoreId { get; set; }
        public Store Store { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public List<InvoiceItem> Items { get; set; } = new();

        public decimal Total => Items?.Sum(i => i.Total) ?? 0;
    }

}
