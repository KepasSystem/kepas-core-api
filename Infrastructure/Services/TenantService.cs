using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kepas.Core.Api.Domain.Entities;
using Kepas.Core.Api.Domain.Interfaces;
using Kepas.Core.Api.Domain.DTOs.Requests;
using Kepas.Core.Api.Domain.DTOs.Responses;
using Kepas.Core.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace Kepas.Core.Api.Infrastructure.Services
{
    public class TenantService : ITenantService
    {
        private readonly AppDbContext _context;

        public TenantService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TenantDTO>> GetAllTenantsAsync()
        {
            var tenants = await _context.Tenants
                .OrderByDescending(t => t.CreatedAt)
                .Select(t => new TenantDTO
                {
                    Id = t.Id,
                    Name = t.Name,
                    Subdomain = t.Subdomain,
                    Email = t.Email,
                    // TODO: IsActive requires fetching if it's on Account or TenantUser. 
                    // Temporarily using true since we didn't add IsActive to Tenant entity directly yet.
                    IsActive = true, 
                    CreatedAt = t.CreatedAt,
                    CurrentPlan = "Básico"
                })
                .ToListAsync();

            return tenants;
        }

        public async Task<TenantDTO> CreateTenantAsync(CreateTenantRequest request)
        {
            // 1. Validar subdomínio único
            if (await _context.Tenants.AnyAsync(t => t.Subdomain == request.Subdomain))
                throw new Exception("Subdomínio já está em uso.");

            // Pegamos o ServiceAccount global por segurança
            var superAdmin = await _context.ServiceAccounts.FirstOrDefaultAsync();
            if (superAdmin == null) throw new Exception("SuperAdmin master account not found.");

            // 2. Criar Tenant
            var tenant = new Tenant
            {
                Id = Guid.NewGuid(),
                AccountId = superAdmin.Id,
                Name = request.Name,
                Subdomain = request.Subdomain,
                Email = request.Email,
                LocalSecretKey = Guid.NewGuid().ToString(),
                LogoUrl = "https://via.placeholder.com/150",
                ThemeColorsJson = "{}"
            };
            _context.Tenants.Add(tenant);

            // 3. Criar Role Admin para o Tenant
            var adminRole = new Role { Id = Guid.NewGuid(), TenantId = tenant.Id, Name = "Admin" };
            _context.Roles.Add(adminRole);

            // 4. Criar Usuário Dono
            var user = new TenantUser
            {
                Id = Guid.NewGuid(),
                TenantId = tenant.Id,
                RoleId = adminRole.Id,
                Name = request.OwnerName ?? "Administrador",
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.OwnerPassword ?? "Mudar123!"),
                IsActive = true
            };
            _context.TenantUsers.Add(user);

            await _context.SaveChangesAsync();

            return new TenantDTO
            {
                Id = tenant.Id,
                Name = tenant.Name,
                Subdomain = tenant.Subdomain,
                Email = tenant.Email,
                IsActive = true,
                CreatedAt = tenant.CreatedAt
            };
        }

        public async Task<bool> ToggleTenantStatusAsync(Guid tenantId)
        {
            // Faremos essa implementação de toggle quando alterarmos o Entity para possuir IsActive
            return await Task.FromResult(true);
        }

        public async Task<TenantDTO> GetTenantBySubdomainAsync(string subdomain)
        {
            var tenant = await _context.Tenants
                .FirstOrDefaultAsync(t => t.Subdomain.ToLower() == subdomain.ToLower());

            if (tenant == null) return null;

            return new TenantDTO
            {
                Id = tenant.Id,
                Name = tenant.Name,
                Subdomain = tenant.Subdomain,
                Email = tenant.Email,
                IsActive = true, // Temporário até adicionar IsActive na Entity Tenant
                CreatedAt = tenant.CreatedAt
            };
        }
    }
}
