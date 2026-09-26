using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Kepas.Core.Api.Domain.Interfaces;
using Kepas.Core.Api.Domain.Entities;
using Kepas.Core.Api.Domain.DTOs.Requests.ServiceAccounts;
using Kepas.Core.Api.Domain.DTOs.Responses.ServiceAccounts;
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

        public async Task<List<ServiceAccountDTO>> GetAllAsync()
        {
            var accounts = await _context.ServiceAccounts
                .Include(a => a.Tenants)
                .Include(a => a.Subscriptions)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return accounts.Select(a => new ServiceAccountDTO
            {
                Id = a.Id,
                OwnerName = a.OwnerName,
                Email = a.Email,
                CreatedAt = a.CreatedAt,
                TotalTenants = a.Tenants.Count,
                TotalSubscriptions = a.Subscriptions.Count
            }).ToList();
        }

        public async Task<ServiceAccountDTO> CreateAsync(CreateServiceAccountRequest request)
        {
            if (await _context.ServiceAccounts.AnyAsync(a => a.Email == request.Email))
            {
                throw new Exception("E-mail já está em uso por outra conta de serviço.");
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
                TotalSubscriptions = 0
            };
        }
    }
}
