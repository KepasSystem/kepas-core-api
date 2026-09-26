using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kepas.Core.Api.Domain.DTOs.Requests.Platform;
using Kepas.Core.Api.Domain.DTOs.Responses.Platform;

namespace Kepas.Core.Api.Domain.Interfaces
{
    public interface IPlatformAdminService
    {
        Task<List<PlatformAdminDTO>> GetAllAdminsAsync();
        Task<PlatformAdminDTO> CreateAdminAsync(CreatePlatformAdminRequest request);
        Task<bool> ToggleAdminStatusAsync(Guid id, bool isActive);
    }
}
