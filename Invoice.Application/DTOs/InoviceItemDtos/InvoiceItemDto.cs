using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Application.DTOs.InoviceItemDtos
{

    public class InvoiceItemDto
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public float Discount { get; set; }
        public float Tax { get; set; }
        public decimal Total { get; set; }
    }

}
