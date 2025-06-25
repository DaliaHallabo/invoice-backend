using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Application.DTOs.InoviceItemDtos
{
    public class CreateInvoiceItemDto
    {
        public int? ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public float Discount { get; set; }
        public float Tax { get; set; }
    }
}
