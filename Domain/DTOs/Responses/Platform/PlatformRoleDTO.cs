using System;
using System.Collections.Generic;

namespace Kepas.Core.Api.Domain.DTOs.Responses.Platform
{
    public class PlatformPermissionDTO
    {
        public Guid Id { get; set; }
        public string Resource { get; set; }
        public string Action { get; set; }
    }

    public class PlatformRoleDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<PlatformPermissionDTO> Permissions { get; set; } = new();
    }
}
