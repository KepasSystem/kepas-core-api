namespace Kepas.Core.Api.Domain.DTOs.Responses
{
    public class SystemConfigDTO
    {
        public string SmtpEmail { get; set; }
        public bool IsSmtpConfigured { get; set; }
        public string WhatsAppSessionId { get; set; }
        public bool IsWhatsAppConnected { get; set; }
    }
}
