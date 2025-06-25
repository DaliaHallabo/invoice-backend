using Invoice.Application.DTOs.InoviceItemDtos;
using Invoice.Application.DTOs.InvoiceDtos;
using Invoice.Application.Interfaces;
using Invoice.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;


namespace Invoice.Application.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IAppDbContext _context;
        private readonly IInvoiceNotifier _notifier;

        public InvoiceService(IAppDbContext context, IInvoiceNotifier notifier)
        {
            _context = context;
            _notifier = notifier;
        }

        public async Task<List<InvoiceDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            return await _context.Invoices
                .OrderByDescending(i => i.Date)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(i => new InvoiceDto
                {
                    Id = i.Id,
                    Serial = i.Serial,
                    CustomerName = i.Customer.Name,
                    StoreName = i.Store.Name,
                    Total = i.Items.Sum(x => x.Price * x.Quantity),
                    CreatedAt = i.Date
                }).ToListAsync();
        }
        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Invoices.CountAsync();
        }

        public async Task<List<InvoiceDto>> GetAllAsync()
        {
            var invoices = await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Store)
                .Include(i => i.Items)
                .ThenInclude(it => it.Product)
                .OrderByDescending(i => i.Id)
                .ToListAsync();

            // Map to DTOs
            return invoices.Select(i => new InvoiceDto
            {
                Id = i.Id,
                Serial = i.Serial,
                CustomerName = i.Customer.Name,
                StoreName = i.Store.Name,
                Total = i.Items.Sum(x => (x.Price * x.Quantity) - (decimal)x.DiscountPercent + (decimal)x.TaxPercent),
                Items = i.Items.Select(item => new InvoiceItemDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    Discount = item.DiscountPercent,
                    Tax = item.TaxPercent
                }).ToList()
            }).ToList();
        }

        public async Task<InvoiceDto> GetByIdAsync(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Items)
                .ThenInclude(i => i.Product)
                .Include(i => i.Customer)
                .Include(i => i.Store)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
                throw new Exception("Invoice not found");

            return new InvoiceDto
            {
                Id = invoice.Id,
                Serial = invoice.Serial,
                CreatedAt = invoice.Date,
                Note = invoice.Note,
                CustomerId = invoice.CustomerId,
                CustomerName = invoice.Customer?.Name,
                StoreId = invoice.StoreId,
                StoreName = invoice.Store?.Name,
                Total = invoice.Items.Sum(item =>
                {
                    var subtotal = item.Price * item.Quantity;
                    var discountAmount = subtotal * (decimal)(item.DiscountPercent / 100f);
                    var taxAmount = (subtotal - discountAmount) * (decimal)(item.TaxPercent / 100f);
                    return subtotal - discountAmount + taxAmount;
                }),
                Items = invoice.Items.Select(item => new InvoiceItemDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product?.Name,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    Discount = item.DiscountPercent,
                    Tax = item.TaxPercent,
                    Total = item.Price * item.Quantity
                }).ToList()
            };
        }




        public async Task CreateAsync(CreateInvoiceDto dto)
        {
            var invoice = new InvoiceApp
            {
                Serial = dto.Serial,
                Note = dto.Note,
                CustomerId = dto.CustomerId,
                StoreId = dto.StoreId,
                Items = dto.Items != null
                    ? dto.Items
                        .Where(i => i.ProductId != null)
                        .Select(i => new InvoiceItem
                        {
                            ProductId = i.ProductId!.Value,
                            Quantity = i.Quantity,
                            Price = i.Price,
                            DiscountPercent = i.Discount,
                            TaxPercent = i.Tax
                        }).ToList()
                    : new List<InvoiceItem>()
            };

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
            await _notifier.NotifyInvoiceListUpdatedAsync();
        }

        public async Task UpdateAsync(int id, CreateInvoiceDto dto)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
                throw new Exception("Invoice not found");

            invoice.Serial = dto.Serial;
            invoice.Note = dto.Note;
            invoice.StoreId = dto.StoreId;
            invoice.CustomerId = dto.CustomerId;

            // Handle invoice items gracefully
            _context.InvoiceItems.RemoveRange(invoice.Items);

            invoice.Items = dto.Items != null
                ? dto.Items
                    .Where(i => i.ProductId != null)
                    .Select(i => new InvoiceItem
                    {
                        ProductId = i.ProductId.Value,
                        Quantity = i.Quantity,
                        Price = i.Price,
                        DiscountPercent = i.Discount,
                        TaxPercent = i.Tax
                    }).ToList()
                : new List<InvoiceItem>();

            await _context.SaveChangesAsync();
        }


        public async Task DeleteAsync(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Items) // Make sure Items is a navigation property
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
                throw new Exception("Invoice not found");

            // First, remove related items
            _context.InvoiceItems.RemoveRange(invoice.Items);

            // Then, remove the invoice
            _context.Invoices.Remove(invoice);

            await _context.SaveChangesAsync();
        }

    }

}
