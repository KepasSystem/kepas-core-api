using System;
using System.Collections.Generic;

namespace Kepas.Core.Api.Domain.Entities
{
    public class Tenant : EntityBase
    {
        public Guid AccountId { get; set; }
        public ServiceAccount Account { get; set; }

        public string Name { get; set; }
        public string Subdomain { get; set; }
        public string Email { get; set; }
        public string LogoUrl { get; set; }
        public string ThemeColorsJson { get; set; }
        public string LocalSecretKey { get; set; } // Para DB local offline
        public bool IsActive { get; set; } = true;

        public ICollection<TenantUser> Users { get; set; } = new List<TenantUser>();
        public ICollection<Role> Roles { get; set; } = new List<Role>();
        public ICollection<ServiceCategory> ServiceCategories { get; set; } = new List<ServiceCategory>();
        public ICollection<Customer> Customers { get; set; } = new List<Customer>();
        public ICollection<PaymentMethodConfig> PaymentMethodConfigs { get; set; } = new List<PaymentMethodConfig>();
        public ICollection<Banner> Banners { get; set; } = new List<Banner>();
        public ICollection<NewsletterSubscriber> NewsletterSubscribers { get; set; } = new List<NewsletterSubscriber>();
    }

    public class Role : EntityBase
    {
        public Guid TenantId { get; set; }
        public Tenant Tenant { get; set; }
        public string Name { get; set; }
        public ICollection<RolePermission> Permissions { get; set; } = new List<RolePermission>();
        public ICollection<TenantUser> Users { get; set; } = new List<TenantUser>();
    }

    public class RolePermission : EntityBase
    {
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
        public string Resource { get; set; }
        public string Action { get; set; }
    }

    public class TenantUser : EntityBase
    {
        public Guid TenantId { get; set; }
        public Tenant Tenant { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
        
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; }
    }
}
