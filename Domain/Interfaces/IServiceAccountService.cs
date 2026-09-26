using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kepas.Core.Api.Domain.DTOs.Requests.ServiceAccounts;
using Kepas.Core.Api.Domain.DTOs.Responses.ServiceAccounts;

namespace Kepas.Core.Api.Domain.Interfaces
{
    public interface IServiceAccountService
    {
        Task<List<ServiceAccountDTO>> GetAllAsync();
        Task<ServiceAccountDTO> CreateAsync(CreateServiceAccountRequest request);
    }
}
