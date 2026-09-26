using Kepas.Core.Api.Domain.Entities;
using BCrypt.Net;

namespace Kepas.Core.Api.Infrastructure.Data
{
    public static class DataSeeder
    {
        public static void SeedData(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.ServiceAccounts.Any())
            {
                var superAdminAccount = new ServiceAccount
                {
                    Id = Guid.NewGuid(),
                    OwnerName = "Super Admin KEPAS (Owner)",
                    Email = "admin@kepas.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123!"),
                    CreatedAt = DateTime.UtcNow
                };
                context.ServiceAccounts.Add(superAdminAccount);
                context.SaveChanges();

                // 1.5 Seed Platform Role and Admin
                var superAdminRole = new PlatformRole
                {
                    Id = Guid.NewGuid(),
                    Name = "Super Administrador",
                    Permissions = new List<PlatformPermission>
                    {
                        new PlatformPermission { Resource = "*", Action = "*" }
                    }
                };
                context.PlatformRoles.Add(superAdminRole);

                var platformAdmin = new PlatformAdmin
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin Global KEPAS",
                    Email = "admin@kepas.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123!"),
                    PlatformRoleId = superAdminRole.Id,
                    IsActive = true
                };
                context.PlatformAdmins.Add(platformAdmin);
                context.SaveChanges();

                var demoTenant = new Tenant
                {
                    Id = Guid.NewGuid(),
                    AccountId = superAdminAccount.Id,
                    Name = "Kepas Eventos Demo",
                    Subdomain = "localhost",
                    Email = "contato@kepaseventos.com",
                    ThemeColorsJson = "{\"primary\": \"#1e40af\", \"secondary\": \"#1e3a8a\"}",
                    LogoUrl = "https://via.placeholder.com/150",
                    CreatedAt = DateTime.UtcNow,
                    LocalSecretKey = Guid.NewGuid().ToString()
                };
                context.Tenants.Add(demoTenant);
                context.SaveChanges();

                var adminRole = new Role
                {
                    Id = Guid.NewGuid(),
                    TenantId = demoTenant.Id,
                    Name = "Admin",
                    CreatedAt = DateTime.UtcNow
                };
                context.Roles.Add(adminRole);
                context.SaveChanges();

                var tenantUser = new TenantUser
                {
                    Id = Guid.NewGuid(),
                    TenantId = demoTenant.Id,
                    RoleId = adminRole.Id,
                    Name = "João Cliente",
                    Email = "joao@kepaseventos.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Cliente@123!"),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };
                context.TenantUsers.Add(tenantUser);
                context.SaveChanges();
            }
        }
    }
}
