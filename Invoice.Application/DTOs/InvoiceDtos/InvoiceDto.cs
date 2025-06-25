using Invoice.Application.DTOs.InoviceItemDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Application.DTOs.InvoiceDtos
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public string Serial { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Note { get; set; }

        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }

        public int StoreId { get; set; }
        public string? StoreName { get; set; }

        public decimal Total { get; set; }
        public List<InvoiceItemDto> Items { get; set; } = new();
    }

}
