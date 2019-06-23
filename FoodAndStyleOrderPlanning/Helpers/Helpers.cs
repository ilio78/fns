using Microsoft.AspNetCore.Http;

namespace FoodAndStyleOrderPlanning.Helpers
{
    public static class User
    {
        public static string GetUPN(HttpContext context)
        {
            var upn = context.Request.Headers["X-MS-CLIENT-PRINCIPAL-NAME"];
            if (string.IsNullOrWhiteSpace(upn))
                return "unknown";
            else
                return upn;
        }
    }
}
