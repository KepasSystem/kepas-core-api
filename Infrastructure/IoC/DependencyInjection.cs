using Kepas.Core.Api.Domain.Interfaces;
using Kepas.Core.Api.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Kepas.Core.Api.Infrastructure.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
        {
            // Register all domain services here so the application does not need to know about the concrete implementations.
            
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITenantService, TenantService>();
            services.AddScoped<ISystemSettingsService, SystemSettingsService>();
            services.AddScoped<IPlatformRoleService, PlatformRoleService>();
            services.AddScoped<IPlatformAdminService, PlatformAdminService>();
            services.AddScoped<IServiceAccountService, ServiceAccountService>();
            services.AddScoped<ISystemAnalyticsService, SystemAnalyticsService>();
            services.AddScoped<ITranslationService, AppTranslationService>();

            return services;
        }
    }
}
