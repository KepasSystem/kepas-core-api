using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Kepas.Core.Api.Domain.Interfaces;
using Kepas.Core.Api.Domain.DTOs.Responses.SystemAnalytics;
using Kepas.Core.Api.Infrastructure.Data;

namespace Kepas.Core.Api.Infrastructure.Services
{
    public class SystemAnalyticsService : ISystemAnalyticsService
    {
        private readonly AppDbContext _context;

        public SystemAnalyticsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<KpiDTO> GetGlobalKpisAsync()
        {
            var today = DateTime.UtcNow.Date;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);

            var totalAccounts = await _context.ServiceAccounts.CountAsync();
            var totalTenants = await _context.Tenants.CountAsync();
            var activeTenants = await _context.Tenants.CountAsync(t => t.IsActive);
            var inactiveTenants = totalTenants - activeTenants;

            // MRR: Sum of SaaS Payments this month that are completed.
            var mrr = await _context.SaaSPayments
                .Where(p => p.Status == "Completed" && p.PaidAt >= startOfMonth)
                .SumAsync(p => p.Amount);

            // Mocked new subscriptions today for demo (since we don't have CreatedAt on Subscription yet)
            var newSubsToday = 0; // Replace with logic if Subscription gets CreatedAt

            return new KpiDTO
            {
                TotalServiceAccounts = totalAccounts,
                TotalTenants = totalTenants,
                ActiveTenants = activeTenants,
                InactiveTenants = inactiveTenants,
                Mrr = mrr,
                NewSubscriptionsToday = newSubsToday
            };
        }

        public async Task<List<GrowthChartItemDTO>> GetGrowthChartAsync()
        {
            var chart = new List<GrowthChartItemDTO>();
            var now = DateTime.UtcNow;

            for (int i = 5; i >= 0; i--)
            {
                var targetMonth = now.AddMonths(-i);
                var startDate = new DateTime(targetMonth.Year, targetMonth.Month, 1);
                var endDate = startDate.AddMonths(1).AddTicks(-1);

                var accountsInMonth = await _context.ServiceAccounts
                    .CountAsync(a => a.CreatedAt >= startDate && a.CreatedAt <= endDate);

                var tenantsInMonth = await _context.Tenants
                    .CountAsync(t => t.CreatedAt >= startDate && t.CreatedAt <= endDate);

                chart.Add(new GrowthChartItemDTO
                {
                    Month = targetMonth.ToString("MMM"), // e.g. "Jan"
                    Accounts = accountsInMonth,
                    Tenants = tenantsInMonth
                });
            }

            return chart;
        }
    }
}
