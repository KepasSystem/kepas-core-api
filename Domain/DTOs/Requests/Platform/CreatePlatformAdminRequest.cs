using System;

namespace Kepas.Core.Api.Domain.DTOs.Requests.Platform
{
    public class CreatePlatformAdminRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Guid? PlatformRoleId { get; set; }
    }
}
