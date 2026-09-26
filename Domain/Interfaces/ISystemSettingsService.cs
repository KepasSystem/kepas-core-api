using System.Threading.Tasks;
using Kepas.Core.Api.Domain.DTOs.Requests;
using Kepas.Core.Api.Domain.DTOs.Responses;

namespace Kepas.Core.Api.Domain.Interfaces
{
    public interface ISystemSettingsService
    {
        Task<SystemConfigDTO> GetSystemConfigAsync();
        Task<bool> UpdateSmtpSettingsAsync(UpdateSmtpRequest request);
        Task<string> GenerateWhatsAppQrCodeAsync();
    }
}
