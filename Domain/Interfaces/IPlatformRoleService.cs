using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kepas.Core.Api.Domain.DTOs.Requests.Platform;
using Kepas.Core.Api.Domain.DTOs.Responses.Platform;

namespace Kepas.Core.Api.Domain.Interfaces
{
    public interface IPlatformRoleService
    {
        Task<List<PlatformRoleDTO>> GetAllRolesAsync();
        Task<PlatformRoleDTO> CreateRoleAsync(CreatePlatformRoleRequest request);
        Task<bool> DeleteRoleAsync(Guid id);
    }
}
