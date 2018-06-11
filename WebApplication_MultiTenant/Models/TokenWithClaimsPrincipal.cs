using System.Security.Claims;

namespace WebApplication_MultiTenant.Models
{
    public class TokenWithClaimsPrincipal
    {
        public string AccessToken { get; set; }

        public ClaimsPrincipal ClaimsPrincipal { get; set; }
    }
}