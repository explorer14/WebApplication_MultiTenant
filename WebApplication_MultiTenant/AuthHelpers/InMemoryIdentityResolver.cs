using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using WebApplication_MultiTenant.Models;

namespace WebApplication_MultiTenant.AuthHelpers
{
    public class InMemoryIdentityResolver : IIdentityResolver
    {
        public static LoginModel[] registeredTenants = null;

        public InMemoryIdentityResolver()
        {
            // only for testing!!!
            if (registeredTenants == null)
            {
                registeredTenants = new LoginModel[2];
                registeredTenants[0] = new LoginModel() { Username = "amanag1983@gmail.com", Password = "test" };
                registeredTenants[1] = new LoginModel() { Username = "aman@yahoo.com", Password = "test123" };
            }
        }

        public ClaimsPrincipal CreateClaimsPrincipal(List<Claim> tokenClaims)
        {
            var claimsPrincipal = new ClaimsPrincipal();
            claimsPrincipal.AddIdentity(new ClaimsIdentity(tokenClaims, "Password", ClaimTypes.Name, "Recipient"));

            return claimsPrincipal;
        }

        public bool IsIdentityConfirmed(string username, string password)
        {
            // only for dev and test. This will a separate API call to the Identity and Access API
            return registeredTenants.Count(x => x.Username == username && x.Password == password) == 1;
        }
    }
}