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
        
                public Domain.Enums.PipelineStatus PipelineStatus { get; set; } = Domain.Enums.PipelineStatus.Prospect;
        public decimal EstimatedValue { get; set; } = 0;

        public ICollection<ServiceAccountNote> Notes { get; set; } = new List<ServiceAccountNote>();
        public ICollection<ServiceAccountDocument> Documents { get; set; } = new List<ServiceAccountDocument>();
        public ICollection<SupportTicket> SupportTickets { get; set; } = new List<SupportTicket>();
        
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
    public class ServiceAccountNote : EntityBase
    {
        public Guid ServiceAccountId { get; set; }
        public ServiceAccount ServiceAccount { get; set; }
        public string Content { get; set; }
        public Domain.Enums.InteractionType InteractionType { get; set; }
        public Guid? AuthorId { get; set; }
        public PlatformAdmin Author { get; set; }
    }

    public class ServiceAccountDocument : EntityBase
    {
        public Guid ServiceAccountId { get; set; }
        public ServiceAccount ServiceAccount { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public Domain.Enums.DocumentType DocumentType { get; set; }
        public DateTime UploadedAt { get; set; }
    }

    public class SupportTicket : EntityBase
    {
        public Guid ServiceAccountId { get; set; }
        public ServiceAccount ServiceAccount { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public Domain.Enums.TicketStatus Status { get; set; }
        public Domain.Enums.TicketPriority Priority { get; set; }
    }
}
