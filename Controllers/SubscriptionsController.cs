using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Kepas.Core.Api.Domain.DTOs.Responses;

namespace Kepas.Core.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize(Roles = "SuperAdmin")]
    public class SubscriptionsController : ControllerBase
    {
        [HttpGet("modules")]
        public IActionResult GetModules()
        {
            var modules = new List<object>
            {
                new { Id = Guid.NewGuid(), Name = "Módulo de Agendamentos", BasePrice = 99.90, IsActive = true },
                new { Id = Guid.NewGuid(), Name = "Módulo de WhatsApp Automático", BasePrice = 149.90, IsActive = true },
                new { Id = Guid.NewGuid(), Name = "Módulo de Pagamento Online", BasePrice = 49.90, IsActive = true }
            };
            return Ok(ApiResponse<List<object>>.Ok(modules, "Módulos base carregados"));
        }

        [HttpGet("plans")]
        public IActionResult GetPlans()
        {
            var plans = new List<object>
            {
                new { Id = Guid.NewGuid(), Name = "Combo Inicial", Price = 120.00, Modules = new[] { "Agendamentos", "Pagamento Online" } },
                new { Id = Guid.NewGuid(), Name = "Combo KEPAS MAX", Price = 250.00, Modules = new[] { "Agendamentos", "WhatsApp", "Pagamento Online" } }
            };
            return Ok(ApiResponse<List<object>>.Ok(plans, "Planos empacotados carregados"));
        }
    }
}
