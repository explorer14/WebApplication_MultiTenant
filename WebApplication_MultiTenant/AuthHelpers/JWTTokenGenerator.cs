using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication_MultiTenant.Models;

namespace WebApplication_MultiTenant.AuthHelpers
{
    public class JWTTokenGenerator : IJwtTokenGenerator
    {
        private readonly IIdentityResolver identityResolver;
        private readonly IConfiguration config;

        public JWTTokenGenerator(IIdentityResolver identityResolver, IConfiguration config)
        {
            this.identityResolver = identityResolver;
            this.config = config;
        }

        public TokenWithClaimsPrincipal GenerateAccessTokenIfIdentityConfirmed(string userName, string password)
        {
            string accessToken = string.Empty;
            List<Claim> claims = new List<Claim>();

            if (this.identityResolver.IsIdentityConfirmed(userName, password))
            {
                // TODO: More claims can be added such as roles, permissions etc. which will be returned by the IA API
                claims.Add(new Claim(ClaimTypes.Name, userName));
                claims.Add(new Claim(JwtRegisteredClaimNames.Sub, userName));
                claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.TimeOfDay.Ticks.ToString(), ClaimValueTypes.Integer64));

                var expiration = TimeSpan.FromMinutes(5);
                SecurityKey signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this.config["Token:SigningKey"]));

                var jwt = new JwtSecurityToken(issuer: this.config["Token:Issuer"],
                                               audience: this.config["Token:Audience"],
                                               claims: claims,
                                               notBefore: DateTime.UtcNow,
                                               expires: DateTime.UtcNow.Add(expiration),
                                               signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));

                accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);
            }

            return new TokenWithClaimsPrincipal() { AccessToken = accessToken, ClaimsPrincipal = this.identityResolver.CreateClaimsPrincipal(claims) };
        }
    }
}