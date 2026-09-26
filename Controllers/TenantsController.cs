using System;
using System.Threading.Tasks;
using Kepas.Core.Api.Domain.Interfaces;
using Kepas.Core.Api.Domain.DTOs.Requests;
using Kepas.Core.Api.Domain.DTOs.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Kepas.Core.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize(Roles = "SuperAdmin")]
    public class TenantsController : ControllerBase
    {
        private readonly ITenantService _tenantService;

        public TenantsController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTenants()
        {
            try
            {
                var tenants = await _tenantService.GetAllTenantsAsync();
                return Ok(ApiResponse<List<TenantDTO>>.Ok(tenants, "Inquilinos carregados com sucesso."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<List<TenantDTO>>.Error(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTenant([FromBody] CreateTenantRequest request)
        {
            try
            {
                var newTenant = await _tenantService.CreateTenantAsync(request);
                return Created($"/api/v1/tenants/{newTenant.Id}", ApiResponse<TenantDTO>.Ok(newTenant, "Inquilino provisionado com sucesso."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<TenantDTO>.Error(ex.Message));
            }
        }

        [HttpGet("resolve")]
        [AllowAnonymous]
        public async Task<IActionResult> ResolveTenant([FromQuery] string subdomain)
        {
            if (string.IsNullOrEmpty(subdomain))
                return BadRequest(ApiResponse<TenantDTO>.Error("Subdomínio não informado."));

            try
            {
                var tenant = await _tenantService.GetTenantBySubdomainAsync(subdomain);
                if (tenant == null)
                    return NotFound(ApiResponse<TenantDTO>.Error("Empresa não encontrada."));
                    
                if (!tenant.IsActive)
                    return BadRequest(ApiResponse<TenantDTO>.Error("Conta suspensa. Contate o suporte."));

                // Retorna apenas dados públicos/inofensivos necessários pro frontend (Logo, Nome, Tema, ID)
                return Ok(ApiResponse<TenantDTO>.Ok(tenant, "Tenant resolvido."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<TenantDTO>.Error("Erro interno ao resolver inquilino."));
            }
        }
    }
}
