using Invoice.Application.DTOs.InoviceItemDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Application.DTOs.InvoiceDtos
{
    public class CreateInvoiceDto
    {
        public string Serial { get; set; }
        public string Note { get; set; }
        public int StoreId { get; set; }
        public int CustomerId { get; set; }
        public List<CreateInvoiceItemDto>? Items { get; set; }
    }
}
