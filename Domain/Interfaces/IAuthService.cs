using System.Threading.Tasks;
using Kepas.Core.Api.Domain.DTOs.Responses;
using Kepas.Core.Api.Domain.DTOs.Requests;

namespace Kepas.Core.Api.Domain.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> LoginAsync(string email, string password);
        Task<AuthResult> TenantLoginAsync(TenantLoginRequest request);
        Task<AuthResult> LoginSuperAdminAsync(string email, string password);
    }
}
