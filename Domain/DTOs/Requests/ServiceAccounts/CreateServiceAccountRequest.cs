using System;

namespace Kepas.Core.Api.Domain.DTOs.Requests.ServiceAccounts
{
    public class CreateServiceAccountRequest
    {
        public string OwnerName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
