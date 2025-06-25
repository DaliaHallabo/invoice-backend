using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Domain.Entities
{
    public class InvoiceItem
    {
        public int Id { get; set; }

        public int InvoiceId { get; set; }
        public InvoiceApp Invoice { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public float DiscountPercent { get; set; }
        public float TaxPercent { get; set; }

        public decimal Total => Quantity * Price * (1 - (decimal)DiscountPercent / 100) * (1 + (decimal)TaxPercent / 100);
    }

}
