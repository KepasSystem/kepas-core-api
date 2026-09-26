using System;
using System.ComponentModel.DataAnnotations;

namespace Kepas.Core.Api.Domain.DTOs.Requests
{
    public class CreateTenantRequest
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Subdomain { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string OwnerName { get; set; } // Nome do usuario admin que será criado
        public string OwnerPassword { get; set; } // Senha inicial
        
        [Required]
        public Guid AccountId { get; set; }
    }
}
