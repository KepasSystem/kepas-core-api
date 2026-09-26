using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kepas.Core.Api.Domain.DTOs.Requests;
using Kepas.Core.Api.Domain.DTOs.Responses;

namespace Kepas.Core.Api.Domain.Interfaces
{
    public interface ITenantService
    {
        Task<List<TenantDTO>> GetAllTenantsAsync();
        Task<TenantDTO> CreateTenantAsync(CreateTenantRequest request);
        Task<bool> ToggleTenantStatusAsync(Guid tenantId);
        Task<TenantDTO> GetTenantBySubdomainAsync(string subdomain);
    }
}
