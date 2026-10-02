using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Collections.Generic;
using Kepas.Core.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Kepas.Core.Api.Infrastructure.Middlewares
{
    public class DynamicCorsMiddleware
    {
        private readonly RequestDelegate _next;

        public DynamicCorsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, Microsoft.Extensions.Configuration.IConfiguration config)
        {
                        var origin = context.Request.Headers["Origin"].ToString();
            var adminPortalDomain = config["AdminSettings:AdminPortalDomain"];
            var path = context.Request.Path.Value?.ToLower() ?? "";

            bool isAdminRoute = path.StartsWith("/api/v1/platform") ||
                                path.StartsWith("/api/v1/serviceaccounts") ||
                                path.StartsWith("/api/v1/tenants") ||
                                path.StartsWith("/api/v1/system-analytics") ||
                                path.StartsWith("/api/v1/systemsettings") ||
                                path.StartsWith("/api/v1/subscriptions") ||
                                path.StartsWith("/api/v1/auth/superadmin");

            bool isDev = origin.Contains("localhost") || origin.Contains("127.0.0.1");
            bool isAdminOrigin = !string.IsNullOrEmpty(adminPortalDomain) && origin.Contains(adminPortalDomain);

            // Se for requisição da mesma origem (ou mobile/insomnia sem origin), ignora bloqueio estrito
            if (string.IsNullOrEmpty(origin) || isDev || isAdminOrigin) 
            {
                if (!string.IsNullOrEmpty(origin))
                {
                    context.Response.Headers.Append("Access-Control-Allow-Origin", origin);
                    context.Response.Headers.Append("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
                    context.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type, Authorization, Accept-Language");
                    if (context.Request.Method == "OPTIONS")
                    {
                        context.Response.StatusCode = 200;
                        return;
                    }
                }
                await _next(context);
                return;
            }

            if (isAdminRoute)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("CORS Policy: Rotas de administracao sao restritas ao portal admin.");
                return;
            }

            // Precisamos criar um Scope porque DbContext nÃ£o pode ser injetado direto num Middleware Singleton
            using var scope = context.RequestServices.CreateScope();
            var cache = scope.ServiceProvider.GetRequiredService<IDistributedCache>();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Busca domÃ­nios permitidos no Redis
            var cacheKey = "AllowedDomains";
            var cachedDomains = await cache.GetStringAsync(cacheKey);
            List<string> allowedDomains;

            if (string.IsNullOrEmpty(cachedDomains))
            {
                // Cache vazio: Bate no banco e atualiza o Redis
                allowedDomains = await dbContext.Tenants.Select(t => t.Subdomain).ToListAsync();
                
                var options = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = System.TimeSpan.FromMinutes(60) };
                await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(allowedDomains), options);
            }
            else
            {
                allowedDomains = JsonSerializer.Deserialize<List<string>>(cachedDomains);
            }

            // Se a origem bater com algum subdomÃ­nio ou domÃ­nio completo registrado:
            bool isAllowed = false;
            foreach (var domain in allowedDomains)
            {
                if (!string.IsNullOrEmpty(domain) && origin.Contains(domain))
                {
                    isAllowed = true;
                    break;
                }
            }

            if (!isAllowed)
            {
                context.Response.StatusCode = 403; // Forbidden
                await context.Response.WriteAsync("CORS Policy: Dominio nao registrado no sistema KEPAS.");
                return;
            }

            // Aplica os cabeÃ§alhos de permissÃ£o dinamicamente
            context.Response.Headers.Append("Access-Control-Allow-Origin", origin);
            context.Response.Headers.Append("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
            context.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type, Authorization, Accept-Language");

            // Se for chamada de pre-flight (OPTIONS), apenas retorna 200 OK
            if (context.Request.Method == "OPTIONS")
            {
                context.Response.StatusCode = 200;
                return;
            }

            await _next(context);
        }
    }
}



