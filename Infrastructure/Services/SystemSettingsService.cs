using System;
using System.Threading.Tasks;
using Kepas.Core.Api.Domain.Entities;
using Kepas.Core.Api.Domain.Interfaces;
using Kepas.Core.Api.Domain.DTOs.Requests;
using Kepas.Core.Api.Domain.DTOs.Responses;
using Kepas.Core.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Kepas.Core.Api.Infrastructure.Services
{
    public class SystemSettingsService : ISystemSettingsService
    {
        private readonly AppDbContext _context;

        public SystemSettingsService(AppDbContext context)
        {
            _context = context;
        }

        private async Task<SystemConfig> GetOrCreateConfigAsync()
        {
            var config = await _context.SystemConfigs.FirstOrDefaultAsync();
            if (config == null)
            {
                config = new SystemConfig { Id = Guid.NewGuid(), SmtpEmail = "", SmtpPassword = "", SystemWhatsAppSessionId = "" };
                _context.SystemConfigs.Add(config);
                await _context.SaveChangesAsync();
            }
            return config;
        }

        public async Task<SystemConfigDTO> GetSystemConfigAsync()
        {
            var config = await GetOrCreateConfigAsync();
            
            return new SystemConfigDTO
            {
                SmtpEmail = config.SmtpEmail,
                IsSmtpConfigured = !string.IsNullOrEmpty(config.SmtpPassword),
                WhatsAppSessionId = config.SystemWhatsAppSessionId,
                IsWhatsAppConnected = !string.IsNullOrEmpty(config.SystemWhatsAppSessionId)
            };
        }

        public async Task<bool> UpdateSmtpSettingsAsync(UpdateSmtpRequest request)
        {
            var config = await GetOrCreateConfigAsync();
            config.SmtpEmail = request.SmtpEmail;
            config.SmtpPassword = request.SmtpPassword; // Deveria ser criptografado em prod
            config.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string> GenerateWhatsAppQrCodeAsync()
        {
            // Mock: No futuro, isso fará um GET/POST real para o Node.js Wpp Engine
            // e retornará o Base64 da imagem QR Code
            await Task.Delay(1000); 
            return "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwAEhQGAhKmMIQAAAABJRU5ErkJggg=="; // Placeholder QR
        }
    }
}
