﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<InvoiceItem> InvoiceItems { get; set; } = new();
    }
}

