namespace WebApplication_MultiTenant.Models
{
    public class JWTResponse
    {
        public string AccessToken { get; set; }

        public int SecondsUntilExpiration { get; set; }
    }
}