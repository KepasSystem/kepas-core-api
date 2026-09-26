using System;
using System.Threading.Tasks;
using Kepas.Core.Api.Domain.Interfaces;
using Kepas.Core.Api.Domain.DTOs.Requests;
using Kepas.Core.Api.Domain.DTOs.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kepas.Core.Api.Controllers
{
    [ApiController]
    [Route("api/v1/system-settings")]
    [Authorize(Roles = "SuperAdmin")]
    public class SystemSettingsController : ControllerBase
    {
        private readonly ISystemSettingsService _settingsService;

        public SystemSettingsController(ISystemSettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSettings()
        {
            try
            {
                var config = await _settingsService.GetSystemConfigAsync();
                return Ok(ApiResponse<SystemConfigDTO>.Ok(config, "Configurações carregadas."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<SystemConfigDTO>.Error(ex.Message));
            }
        }

        [HttpPut("smtp")]
        public async Task<IActionResult> UpdateSmtp([FromBody] UpdateSmtpRequest request)
        {
            try
            {
                await _settingsService.UpdateSmtpSettingsAsync(request);
                return Ok(ApiResponse<bool>.Ok(true, "SMTP atualizado com sucesso."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.Error(ex.Message));
            }
        }

        [HttpGet("whatsapp/qrcode")]
        public async Task<IActionResult> GetWhatsAppQrCode()
        {
            try
            {
                var base64Qr = await _settingsService.GenerateWhatsAppQrCodeAsync();
                return Ok(ApiResponse<string>.Ok(base64Qr, "QR Code gerado. Aguardando leitura..."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.Error(ex.Message));
            }
        }
    }
}
