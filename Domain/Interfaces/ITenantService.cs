using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kepas.Core.Api.Domain.DTOs.Requests;
using Kepas.Core.Api.Domain.DTOs.Responses;
using Kepas.Core.Api.Domain.DTOs.Responses.Pagination;

namespace Kepas.Core.Api.Domain.Interfaces
{
    public interface ITenantService
    {
        Task<PagedResult<TenantDTO>> GetAllTenantsAsync(string search, int page, int limit);
        Task<TenantDTO> CreateTenantAsync(CreateTenantRequest request);
        Task<bool> ToggleTenantStatusAsync(Guid tenantId);
        Task<TenantDTO> GetTenantBySubdomainAsync(string subdomain);
    }
}
