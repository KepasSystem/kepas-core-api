using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Kepas.Core.Api.Infrastructure.Middlewares
{
    public class TenantResolutionMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantResolutionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Tenta pegar o TenantId do token JWT primeiro (se o usuário estiver logado)
            var tenantClaim = context.User.Claims.FirstOrDefault(c => c.Type == "TenantId");
            
            if (tenantClaim != null && Guid.TryParse(tenantClaim.Value, out Guid tenantId))
            {
                context.Items["TenantId"] = tenantId;
            }
            else
            {
                // Se não estiver logado, podemos tentar inferir pelo domínio (Origin ou Host) 
                // para rotas públicas (ex: carregar os Banners do site público do cliente)
                var host = context.Request.Headers["Host"].ToString();
                var origin = context.Request.Headers["Origin"].ToString();
                
                // A lógica completa aqui seria buscar no Cache qual TenantId pertence a esse host/origin
                // context.Items["TenantId"] = idDoCache;
            }

            await _next(context);
        }
    }
}
