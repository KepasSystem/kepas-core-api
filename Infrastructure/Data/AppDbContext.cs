using Kepas.Core.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kepas.Core.Api.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<SystemConfig> SystemConfigs { get; set; }
        public DbSet<ServiceAccount> ServiceAccounts { get; set; }
        public DbSet<ServiceAccountNote> ServiceAccountNotes { get; set; }
        public DbSet<ServiceAccountDocument> ServiceAccountDocuments { get; set; }
        public DbSet<SupportTicket> SupportTickets { get; set; }
        public DbSet<PlatformAdmin> PlatformAdmins { get; set; }
        public DbSet<PlatformRole> PlatformRoles { get; set; }
        public DbSet<PlatformPermission> PlatformPermissions { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<TenantUser> TenantUsers { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SaaSPayment> SaaSPayments { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<PaymentMethodConfig> PaymentMethodConfigs { get; set; }
        public DbSet<ServiceCategory> ServiceCategories { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<CustomerPayment> CustomerPayments { get; set; }
        public DbSet<MessageLog> MessageLogs { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<NewsletterSubscriber> NewsletterSubscribers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Tenant global query filter for multi-tenancy could be added here later
            // e.g., modelBuilder.Entity<Customer>().HasQueryFilter(c => c.TenantId == CurrentTenantId);

            modelBuilder.Entity<Tenant>()
                .HasOne(t => t.Account)
                .WithMany(a => a.Tenants)
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Customer)
                .WithMany(c => c.Bookings)
                .HasForeignKey(b => b.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
                
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Service)
                .WithMany(s => s.Bookings)
                .HasForeignKey(b => b.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

