using System;

namespace Kepas.Core.Api.Domain.DTOs.Responses
{
    public class TenantDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Subdomain { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CurrentPlan { get; set; } // Opcional por enquanto
    }
}
