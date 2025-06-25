using Invoice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Application.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<InvoiceApp> Invoices { get; }
        DbSet<InvoiceItem> InvoiceItems { get; }
        DbSet<Product> Products { get; }
        DbSet<Customer> Customers { get; }
        DbSet<Store> Stores { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
