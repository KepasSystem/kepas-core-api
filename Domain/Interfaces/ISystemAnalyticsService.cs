using System.Collections.Generic;
using System.Threading.Tasks;
using Kepas.Core.Api.Domain.DTOs.Responses.SystemAnalytics;

namespace Kepas.Core.Api.Domain.Interfaces
{
    public interface ISystemAnalyticsService
    {
        Task<KpiDTO> GetGlobalKpisAsync();
        Task<List<GrowthChartItemDTO>> GetGrowthChartAsync();
    }
}
