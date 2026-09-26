using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Kepas.Core.Api.Domain.Interfaces;
using Kepas.Core.Api.Domain.Entities;
using Kepas.Core.Api.Domain.DTOs.Requests.Platform;
using Kepas.Core.Api.Domain.DTOs.Responses.Platform;
using Kepas.Core.Api.Infrastructure.Data;

namespace Kepas.Core.Api.Infrastructure.Services
{
    public class PlatformAdminService : IPlatformAdminService
    {
        private readonly AppDbContext _context;

        public PlatformAdminService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PlatformAdminDTO>> GetAllAdminsAsync()
        {
            var admins = await _context.PlatformAdmins
                .Include(a => a.PlatformRole)
                .ThenInclude(r => r.Permissions)
                .ToListAsync();

            return admins.Select(a => new PlatformAdminDTO
            {
                Id = a.Id,
                Name = a.Name,
                Email = a.Email,
                IsActive = a.IsActive,
                Role = a.PlatformRole != null ? new PlatformRoleDTO
                {
                    Id = a.PlatformRole.Id,
                    Name = a.PlatformRole.Name,
                    Permissions = a.PlatformRole.Permissions.Select(p => new PlatformPermissionDTO
                    {
                        Id = p.Id,
                        Resource = p.Resource,
                        Action = p.Action
                    }).ToList()
                } : null
            }).ToList();
        }

        public async Task<PlatformAdminDTO> CreateAdminAsync(CreatePlatformAdminRequest request)
        {
            var admin = new PlatformAdmin
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                PlatformRoleId = request.PlatformRoleId
            };

            _context.PlatformAdmins.Add(admin);
            await _context.SaveChangesAsync();

            return new PlatformAdminDTO
            {
                Id = admin.Id,
                Name = admin.Name,
                Email = admin.Email,
                IsActive = admin.IsActive
            };
        }

        public async Task<bool> ToggleAdminStatusAsync(Guid id, bool isActive)
        {
            var admin = await _context.PlatformAdmins.FindAsync(id);
            if (admin == null) return false;

            admin.IsActive = isActive;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
