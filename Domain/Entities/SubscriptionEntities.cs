using System;
using System.Collections.Generic;

namespace Kepas.Core.Api.Domain.Entities
{
    public class Subscription : EntityBase
    {
        public Guid AccountId { get; set; }
        public ServiceAccount Account { get; set; }
        
        public string PlanName { get; set; }
        public string Status { get; set; } // Active, Suspended
        public DateTime ExpiryDate { get; set; }
        
        public ICollection<SaaSPayment> Payments { get; set; } = new List<SaaSPayment>();
        public ICollection<Module> Modules { get; set; } = new List<Module>();
    }

    public class SaaSPayment : EntityBase
    {
        public Guid SubscriptionId { get; set; }
        public Subscription Subscription { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaidAt { get; set; }
        public string Status { get; set; } // Completed, Failed
    }

    public class Module : EntityBase
    {
        public Guid SubscriptionId { get; set; }
        public Subscription Subscription { get; set; }
        public string Name { get; set; } // e.g. WhatsApp, Booking
        public bool IsActive { get; set; }
    }
}
