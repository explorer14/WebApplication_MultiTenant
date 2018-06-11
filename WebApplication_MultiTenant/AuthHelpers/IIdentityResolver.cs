using System.Collections.Generic;
using System.Security.Claims;

namespace WebApplication_MultiTenant.AuthHelpers
{
    public interface IIdentityResolver
    {
        bool IsIdentityConfirmed(string username, string password);

        ClaimsPrincipal CreateClaimsPrincipal(List<Claim> tokenClaims);
    }
}