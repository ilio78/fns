using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
