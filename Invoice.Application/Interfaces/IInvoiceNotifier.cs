using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Application.Interfaces
{
    public interface IInvoiceNotifier
    {
        Task NotifyInvoiceListUpdatedAsync();
    }
}

