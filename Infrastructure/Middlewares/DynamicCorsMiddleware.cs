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

            // Se for requisição da mesma origem (ou mobile/insomnia sem origin), ignora bloqueio estrito
            if (string.IsNullOrEmpty(origin) || origin.Contains("localhost") || (!string.IsNullOrEmpty(adminPortalDomain) && origin.Contains(adminPortalDomain))) 
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

            // Precisamos criar um Scope porque DbContext não pode ser injetado direto num Middleware Singleton
            using var scope = context.RequestServices.CreateScope();
            var cache = scope.ServiceProvider.GetRequiredService<IDistributedCache>();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Busca domínios permitidos no Redis
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

            // Se a origem bater com algum subdomínio ou domínio completo registrado:
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

            // Aplica os cabeçalhos de permissão dinamicamente
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

