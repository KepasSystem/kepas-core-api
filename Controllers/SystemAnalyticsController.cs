using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kepas.Core.Api.Domain.Interfaces;
using Kepas.Core.Api.Domain.DTOs.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kepas.Core.Api.Controllers
{
    [ApiController]
    [Route("api/v1/system-analytics")]
    [Authorize(Roles = "SuperAdmin")]
    public class SystemAnalyticsController : ControllerBase
    {
        [HttpGet("kpis")]
        public IActionResult GetKpis()
        {
            // Retornando mock rápido para fechar o portal. O ideal é o serviço somar a tabela Tenants
            var kpi = new 
            {
                TotalTenants = 1,
                Mrr = 1500.00,
                ChurnRate = 0.0,
                NewSubscriptionsToday = 1
            };
            return Ok(ApiResponse<object>.Ok(kpi, "KPIs carregados"));
        }

        [HttpGet("growth-chart")]
        public IActionResult GetGrowthChart()
        {
            var chart = new List<object>
            {
                new { Month = "Jan", Clients = 0 },
                new { Month = "Fev", Clients = 0 },
                new { Month = "Mar", Clients = 0 },
                new { Month = "Abr", Clients = 0 },
                new { Month = "Mai", Clients = 0 },
                new { Month = "Jun", Clients = 0 },
                new { Month = "Jul", Clients = 0 },
                new { Month = "Ago", Clients = 0 },
                new { Month = "Set", Clients = 1 }
            };
            return Ok(ApiResponse<List<object>>.Ok(chart, "Gráfico carregado"));
        }
    }
}
