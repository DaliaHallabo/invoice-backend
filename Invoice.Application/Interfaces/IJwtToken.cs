using Invoice.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Application.Interfaces
{
    public interface IJwtToken
    {
        string CreateTokenAsync(ApplicationUser user);
        string GenerateResetToken(ApplicationUser user);
        ClaimsPrincipal? ValidateResetToken(string token);
    }
}
