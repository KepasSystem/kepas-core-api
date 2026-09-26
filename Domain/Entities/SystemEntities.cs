using System;
using System.Collections.Generic;

namespace Kepas.Core.Api.Domain.Entities
{
    public abstract class EntityBase
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }

    public class SystemConfig : EntityBase
    {
        public string SmtpEmail { get; set; }
        public string SmtpPassword { get; set; }
        public string SystemWhatsAppSessionId { get; set; }
        public bool Is2FaEnabledGlobally { get; set; }
    }

    public class ServiceAccount : EntityBase
    {
        public string OwnerName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        
        public ICollection<Tenant> Tenants { get; set; } = new List<Tenant>();
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }

    public class PlatformRole : EntityBase
    {
        public string Name { get; set; } // e.g. "SuperAdmin", "Support", "Billing"
        public ICollection<PlatformPermission> Permissions { get; set; } = new List<PlatformPermission>();
        public ICollection<PlatformAdmin> Admins { get; set; } = new List<PlatformAdmin>();
    }

    public class PlatformPermission : EntityBase
    {
        public Guid PlatformRoleId { get; set; }
        public PlatformRole PlatformRole { get; set; }
        public string Resource { get; set; } // e.g. "Tenants", "SystemConfigs"
        public string Action { get; set; } // e.g. "Read", "Write", "Delete"
    }

    public class PlatformAdmin : EntityBase
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; } = true;
        
        public Guid? PlatformRoleId { get; set; }
        public PlatformRole PlatformRole { get; set; }
    }
}
