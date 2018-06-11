using WebApplication_MultiTenant.Models;

namespace WebApplication_MultiTenant.AuthHelpers
{
    public interface IJwtTokenGenerator
    {
        TokenWithClaimsPrincipal GenerateAccessTokenIfIdentityConfirmed(string userName, string password);
    }
}