using System.ComponentModel.DataAnnotations;

namespace Kepas.Core.Api.Domain.DTOs.Requests
{
    public class TenantLoginRequest
    {
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        [Required]
        public string Subdomain { get; set; }
    }
}
