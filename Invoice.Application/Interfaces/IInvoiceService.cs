using Invoice.Application.DTOs.InvoiceDtos;

namespace Invoice.Application.Interfaces
{
    public interface IInvoiceService
    {
        Task<List<InvoiceDto>> GetAllAsync();
        Task<List<InvoiceDto>> GetPagedAsync(int pageNumber, int pageSize);
        Task<int> GetTotalCountAsync();

        Task<InvoiceDto> GetByIdAsync(int id);
        Task CreateAsync(CreateInvoiceDto dto);
        Task UpdateAsync(int id, CreateInvoiceDto dto);
        Task DeleteAsync(int id);
    }

}
