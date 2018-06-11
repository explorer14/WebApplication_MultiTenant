using DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication_MultiTenant.CustomMiddleware
{
    public class TenantDBMappingMiddleware
    {
        private readonly RequestDelegate next;

        public TenantDBMappingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            string[] urlParts = null;
#if DEBUG
            urlParts = httpContext.Request.Path.Value.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
#else
            urlParts = httpContext.Request.Host.Host.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
#endif

            if (urlParts != null && urlParts.Any())
            {
                httpContext.RequestServices.GetService<IDbContextFactory>().TenantName = urlParts[0];
            }

            await this.next(httpContext);
        }
    }

    public static class TenantDBMappingMiddlewareExtensions
    {
        public static IApplicationBuilder UseTenantDBMapper(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<TenantDBMappingMiddleware>();
        }
    }
}