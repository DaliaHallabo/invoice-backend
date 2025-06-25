using Invoice.Application.Interfaces;
using Invoice.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Infrastructure.Authentication
{
    public class JwtToken : IJwtToken
    {
        private readonly IConfiguration _configuration;

        public JwtToken(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateTokenAsync(ApplicationUser user)
        {
            var AuthClaims = new List<Claim>()
            {
               new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
               new Claim(ClaimTypes.Email,user.Email!),
               new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"]!));

            var token = new JwtSecurityToken(
                 issuer: _configuration["JWT:Issuer"],
                 audience: _configuration["JWT:Audience"],
                 expires: DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["JWT:Expiration"])),
                 claims: AuthClaims,
                 signingCredentials: new SigningCredentials(AuthKey, SecurityAlgorithms.HmacSha256Signature)
                 );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateResetToken(ApplicationUser user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim("ResetTokenPurpose", "PasswordReset")
            };

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"]!));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.UtcNow.AddHours(2),
                claims: claims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal? ValidateResetToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"]!));

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = _configuration["JWT:Issuer"],
                    ValidAudience = _configuration["JWT:Audience"],
                    IssuerSigningKey = authKey
                }, out _);

                var purpose = principal.FindFirst("ResetTokenPurpose")?.Value;
                if (purpose != "PasswordReset")
                    throw new SecurityTokenException("Invalid token purpose.");

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
