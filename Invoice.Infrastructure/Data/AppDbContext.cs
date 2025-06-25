using Invoice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Invoice.Application.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Invoice.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>, IAppDbContext
    {
        public DbSet<InvoiceApp> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Product> Products { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            // Fluent configurations
            modelBuilder.Entity<InvoiceItem>()
     .HasOne(ii => ii.Invoice)
     .WithMany(i => i.Items)
     .HasForeignKey(ii => ii.InvoiceId)
     .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
