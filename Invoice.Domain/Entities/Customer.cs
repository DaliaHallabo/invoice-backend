﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Domain.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<InvoiceApp> Invoices { get; set; } = new();
    }
}
