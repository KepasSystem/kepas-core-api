namespace Kepas.Core.Api.Domain.DTOs.Responses.SystemAnalytics
{
    public class KpiDTO
    {
        public int TotalServiceAccounts { get; set; }
        public int TotalTenants { get; set; }
        public int ActiveTenants { get; set; }
        public int InactiveTenants { get; set; }
        public decimal Mrr { get; set; }
        public int NewSubscriptionsToday { get; set; }
    }
}
