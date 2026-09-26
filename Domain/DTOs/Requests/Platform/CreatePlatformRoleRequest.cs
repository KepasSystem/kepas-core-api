using System;
using System.Collections.Generic;

namespace Kepas.Core.Api.Domain.DTOs.Requests.Platform
{
    public class PlatformPermissionRequest
    {
        public string Resource { get; set; }
        public string Action { get; set; }
    }

    public class CreatePlatformRoleRequest
    {
        public string Name { get; set; }
        public List<PlatformPermissionRequest> Permissions { get; set; } = new();
    }
}
