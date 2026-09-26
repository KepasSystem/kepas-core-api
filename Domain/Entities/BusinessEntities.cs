using System;
using System.Collections.Generic;

namespace Kepas.Core.Api.Domain.Entities
{
    public class PaymentMethodConfig : EntityBase
    {
        public Guid TenantId { get; set; }
        public Tenant Tenant { get; set; }
        public string Provider { get; set; } // Stripe, PayPal, etc.
        public string ApiKeysJson { get; set; }
        public bool IsActive { get; set; }
    }

    public class ServiceCategory : EntityBase
    {
        public Guid TenantId { get; set; }
        public Tenant Tenant { get; set; }
        public string Name { get; set; }
        public ICollection<Service> Services { get; set; } = new List<Service>();
    }

    public class Service : EntityBase
    {
        public Guid CategoryId { get; set; }
        public ServiceCategory Category { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }

    public class Customer : EntityBase
    {
        public Guid TenantId { get; set; }
        public Tenant Tenant { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<MessageLog> MessageLogs { get; set; } = new List<MessageLog>();
    }

    public class Booking : EntityBase
    {
        public Guid TenantId { get; set; }
        public Tenant Tenant { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        public Guid ServiceId { get; set; }
        public Service Service { get; set; }
        
        public DateTime EventDate { get; set; }
        public string Status { get; set; } // Pending, Confirmed, Completed
        public decimal TotalAmount { get; set; }

        public ICollection<CustomerPayment> Payments { get; set; } = new List<CustomerPayment>();
    }

    public class CustomerPayment : EntityBase
    {
        public Guid BookingId { get; set; }
        public Booking Booking { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaidAt { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
    }

    public class MessageLog : EntityBase
    {
        public Guid TenantId { get; set; }
        public Tenant Tenant { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }

        public string Type { get; set; } // WhatsApp, SMS
        public string Provider { get; set; }
        public string Direction { get; set; } // Inbound, Outbound
        public string Content { get; set; }
        public string Status { get; set; } // Sent, Scheduled, Failed
        public DateTime? ScheduledFor { get; set; }
        public DateTime? SentAt { get; set; }
    }

    public class Banner : EntityBase
    {
        public Guid TenantId { get; set; }
        public Tenant Tenant { get; set; }
        public string ImageUrl { get; set; }
        public string TargetLink { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class NewsletterSubscriber : EntityBase
    {
        public Guid TenantId { get; set; }
        public Tenant Tenant { get; set; }
        public string Email { get; set; }
        public DateTime SubscribedAt { get; set; }
    }
}
