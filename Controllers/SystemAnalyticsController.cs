using System.Collections.Generic;
using System.Threading.Tasks;
using Kepas.Core.Api.Domain.Interfaces;
using Kepas.Core.Api.Domain.DTOs.Responses;
using Kepas.Core.Api.Domain.DTOs.Responses.SystemAnalytics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kepas.Core.Api.Controllers
{
    [ApiController]
    [Route("api/v1/system-analytics")]
    [Authorize(Roles = "Super Administrador")]
    public class SystemAnalyticsController : ControllerBase
    {
        private readonly ISystemAnalyticsService _analyticsService;

        public SystemAnalyticsController(ISystemAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [HttpGet("kpis")]
        public async Task<IActionResult> GetKpis()
        {
            var kpis = await _analyticsService.GetGlobalKpisAsync();
            return Ok(ApiResponse<KpiDTO>.Ok(kpis, "KPIs carregados"));
        }

        [HttpGet("growth-chart")]
        public async Task<IActionResult> GetGrowthChart()
        {
            var chart = await _analyticsService.GetGrowthChartAsync();
            return Ok(ApiResponse<List<GrowthChartItemDTO>>.Ok(chart, "Gráfico carregado"));
        }
    }
}
