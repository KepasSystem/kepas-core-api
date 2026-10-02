using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Kepas.Core.Api.Domain.Interfaces;
using Kepas.Core.Api.Domain.Entities;
using Kepas.Core.Api.Domain.DTOs.Requests.ServiceAccounts;
using Kepas.Core.Api.Domain.DTOs.Responses.ServiceAccounts;
using Kepas.Core.Api.Domain.DTOs.Responses.Pagination;
using Kepas.Core.Api.Infrastructure.Data;

namespace Kepas.Core.Api.Infrastructure.Services
{
    public class ServiceAccountService : IServiceAccountService
    {
        private readonly AppDbContext _context;

        public ServiceAccountService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<ServiceAccountDTO>> GetAllAsync(string search, int page, int limit)
        {
            var query = _context.ServiceAccounts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.ToLower();
                query = query.Where(a => a.OwnerName.ToLower().Contains(s) || a.Email.ToLower().Contains(s));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(a => a.CreatedAt)
                .Skip((page - 1) * limit)
                .Take(limit)
                .Select(a => new ServiceAccountDTO
                {
                    Id = a.Id,
                    OwnerName = a.OwnerName,
                    Email = a.Email,
                    CreatedAt = a.CreatedAt,
                    TotalTenants = a.Tenants.Count,
                    TotalSubscriptions = a.Subscriptions.Count,
                    PipelineStatus = a.PipelineStatus,
                    EstimatedValue = a.EstimatedValue
                })
                .ToListAsync();

            return new PagedResult<ServiceAccountDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = page,
                PageSize = limit
            };
        }

        public async Task<ServiceAccountDTO> CreateAsync(CreateServiceAccountRequest request)
        {
            if (await _context.ServiceAccounts.AnyAsync(a => a.Email == request.Email))
            {
                throw new Exception("E-mail jÃ¡ estÃ¡ em uso por outra conta de serviÃ§o.");
            }

            var account = new ServiceAccount
            {
                OwnerName = request.OwnerName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            _context.ServiceAccounts.Add(account);
            await _context.SaveChangesAsync();

            return new ServiceAccountDTO
            {
                Id = account.Id,
                OwnerName = account.OwnerName,
                Email = account.Email,
                CreatedAt = account.CreatedAt,
                TotalTenants = 0,
                TotalSubscriptions = 0,
                PipelineStatus = account.PipelineStatus,
                EstimatedValue = account.EstimatedValue
            };
        }

        public async Task<bool> UpdatePipelineAsync(Guid id, Domain.Enums.PipelineStatus status, decimal estimatedValue)
        {
            var account = await _context.ServiceAccounts.FindAsync(id);
            if (account == null) throw new Exception("Conta de serviço não encontrada.");

            account.PipelineStatus = status;
            account.EstimatedValue = estimatedValue;
            await _context.SaveChangesAsync();
            return true;
        }
        }
}
