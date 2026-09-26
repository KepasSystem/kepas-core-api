using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kepas.Core.Api.Domain.Entities;
using Kepas.Core.Api.Domain.Interfaces;
using Kepas.Core.Api.Domain.DTOs.Requests;
using Kepas.Core.Api.Domain.DTOs.Responses;
using Kepas.Core.Api.Domain.DTOs.Responses.Pagination;
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

        public async Task<PagedResult<TenantDTO>> GetAllTenantsAsync(string search, int page, int limit)
        {
            var query = _context.Tenants.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.ToLower();
                query = query.Where(t => t.Name.ToLower().Contains(s) || t.Subdomain.ToLower().Contains(s) || t.Email.ToLower().Contains(s));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(t => t.CreatedAt)
                .Skip((page - 1) * limit)
                .Take(limit)
                .Select(t => new TenantDTO
                {
                    Id = t.Id,
                    Name = t.Name,
                    Subdomain = t.Subdomain,
                    Email = t.Email,
                    IsActive = t.IsActive, 
                    CreatedAt = t.CreatedAt,
                    CurrentPlan = "Básico"
                })
                .ToListAsync();

            return new PagedResult<TenantDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = page,
                PageSize = limit
            };
        }

        public async Task<TenantDTO> CreateTenantAsync(CreateTenantRequest request)
        {
            // 1. Validar subdomínio único
            if (await _context.Tenants.AnyAsync(t => t.Subdomain == request.Subdomain))
                throw new Exception("Subdomínio já está em uso.");

            // Pegamos o ServiceAccount passado no request
            var serviceAccount = await _context.ServiceAccounts.FindAsync(request.AccountId);
            if (serviceAccount == null) throw new Exception("Conta de serviço (ServiceAccount) não encontrada.");

            // 2. Criar Tenant
            var tenant = new Tenant
            {
                Id = Guid.NewGuid(),
                AccountId = serviceAccount.Id,
                Name = request.Name,
                Subdomain = request.Subdomain,
                Email = request.Email,
                LocalSecretKey = Guid.NewGuid().ToString(),
                LogoUrl = "https://via.placeholder.com/150",
                ThemeColorsJson = "{}",
                IsActive = true
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
                IsActive = tenant.IsActive,
                CreatedAt = tenant.CreatedAt
            };
        }

        public async Task<bool> ToggleTenantStatusAsync(Guid tenantId)
        {
            var tenant = await _context.Tenants.FindAsync(tenantId);
            if (tenant == null) throw new Exception("Inquilino não encontrado.");

            tenant.IsActive = !tenant.IsActive;
            
            // Opcional: Se bloquear o tenant, poderiamos também desativar os utilizadores dele
            
            await _context.SaveChangesAsync();
            return true;
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
                IsActive = tenant.IsActive,
                CreatedAt = tenant.CreatedAt
            };
        }
    }
}
