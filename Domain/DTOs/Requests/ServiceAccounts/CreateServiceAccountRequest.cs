using System;
using System.ComponentModel.DataAnnotations;

namespace Kepas.Core.Api.Domain.DTOs.Requests.ServiceAccounts
{
    public class CreateServiceAccountRequest
    {
        [Required]
        [StringLength(100)]
        public string OwnerName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
