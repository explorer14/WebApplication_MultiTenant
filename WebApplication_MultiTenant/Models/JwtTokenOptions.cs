using Microsoft.IdentityModel.Tokens;

namespace WebApplication_MultiTenant.Models
{
    public class JwtTokenOptions
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public SymmetricSecurityKey SigningKey { get; set; }
    }
}