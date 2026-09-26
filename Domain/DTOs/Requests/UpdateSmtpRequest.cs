using System.ComponentModel.DataAnnotations;

namespace Kepas.Core.Api.Domain.DTOs.Requests
{
    public class UpdateSmtpRequest
    {
        [Required]
        public string SmtpEmail { get; set; }
        
        [Required]
        public string SmtpPassword { get; set; }
    }
}
