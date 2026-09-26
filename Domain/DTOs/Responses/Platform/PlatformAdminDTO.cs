using System;

namespace Kepas.Core.Api.Domain.DTOs.Responses.Platform
{
    public class PlatformAdminDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public PlatformRoleDTO Role { get; set; }
    }
}
