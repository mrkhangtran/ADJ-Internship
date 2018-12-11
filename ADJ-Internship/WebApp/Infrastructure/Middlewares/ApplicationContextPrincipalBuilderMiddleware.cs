using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ADJ.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal;

namespace ADJ.WebApp.Infrastructure.Middlewares
{
    public class ApplicationContextPrincipalBuilderMiddleware
    {
        private readonly RequestDelegate _next;

        public ApplicationContextPrincipalBuilderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var ctx = (ApplicationContext)context.RequestServices.GetService(typeof(ApplicationContext));

            if (context.User.Identity.IsAuthenticated)
            {
                foreach (var claim in context.User.Claims)
                {
                    switch (claim.Type)
                    {
                        case ClaimTypes.Name:
                            ctx.Principal.Username = claim.Value;
                            break;
                        case ClaimTypes.NameIdentifier:
                            ctx.Principal.UserId = claim.Value;
                            break;
                        case ClaimTypes.Role:
                            ctx.Principal.Role = claim.Value;
                            break;
                    }
                }
            }
            else
            {
                ctx.Principal.Username = "anonymous";
            }

            await _next.Invoke(context);
        }
    }
}
