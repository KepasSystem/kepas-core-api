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
    public class PlatformRoleService : IPlatformRoleService
    {
        private readonly AppDbContext _context;

        public PlatformRoleService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PlatformRoleDTO>> GetAllRolesAsync()
        {
            var roles = await _context.PlatformRoles
                .Include(r => r.Permissions)
                .ToListAsync();

            return roles.Select(r => new PlatformRoleDTO
            {
                Id = r.Id,
                Name = r.Name,
                Permissions = r.Permissions.Select(p => new PlatformPermissionDTO
                {
                    Id = p.Id,
                    Resource = p.Resource,
                    Action = p.Action
                }).ToList()
            }).ToList();
        }

        public async Task<PlatformRoleDTO> CreateRoleAsync(CreatePlatformRoleRequest request)
        {
            var role = new PlatformRole
            {
                Name = request.Name,
                Permissions = request.Permissions.Select(p => new PlatformPermission
                {
                    Resource = p.Resource,
                    Action = p.Action
                }).ToList()
            };

            _context.PlatformRoles.Add(role);
            await _context.SaveChangesAsync();

            return new PlatformRoleDTO
            {
                Id = role.Id,
                Name = role.Name,
                Permissions = role.Permissions.Select(p => new PlatformPermissionDTO
                {
                    Id = p.Id,
                    Resource = p.Resource,
                    Action = p.Action
                }).ToList()
            };
        }

        public async Task<bool> DeleteRoleAsync(Guid id)
        {
            var role = await _context.PlatformRoles.FindAsync(id);
            if (role == null) return false;

            _context.PlatformRoles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
