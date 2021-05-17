using Microsoft.AspNetCore.Http;
using System.Linq;

namespace FinanceManager.API.Extensions
{
    public static class HttpExtensions
    {
        public static string GetUserId(this HttpContext httpContext)
        {
            if (httpContext.User == null)
                return string.Empty;

            return httpContext.User.Claims.Single(c => c.Type == "id").Value;
        }
    }
}
