using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kepas.Core.Api.Domain.DTOs.Requests.ServiceAccounts;
using Kepas.Core.Api.Domain.DTOs.Responses.ServiceAccounts;
using Kepas.Core.Api.Domain.DTOs.Responses.Pagination;

namespace Kepas.Core.Api.Domain.Interfaces
{
    public interface IServiceAccountService
    {
        Task<PagedResult<ServiceAccountDTO>> GetAllAsync(string search, int page, int limit);
        Task<ServiceAccountDTO> CreateAsync(CreateServiceAccountRequest request);
        Task<bool> UpdatePipelineAsync(Guid id, Domain.Enums.PipelineStatus status, decimal estimatedValue);
    }
}

