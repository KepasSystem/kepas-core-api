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
    [Authorize(Roles = "Super Administrador")]
    public class TenantsController : ControllerBase
    {
        private readonly ITenantService _tenantService;

        public TenantsController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTenants([FromQuery] string search = "", [FromQuery] int page = 1, [FromQuery] int limit = 10)
        {
            try
            {
                var tenants = await _tenantService.GetAllTenantsAsync(search, page, limit);
                return Ok(ApiResponse<Kepas.Core.Api.Domain.DTOs.Responses.Pagination.PagedResult<TenantDTO>>.Ok(tenants, _translator.GetString(Kepas.Core.Api.Domain.Constants.TranslationKeys.TenantsLoadedSuccess)));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<Kepas.Core.Api.Domain.DTOs.Responses.Pagination.PagedResult<TenantDTO>>.Error(_translator.GetString(Kepas.Core.Api.Domain.Constants.TranslationKeys.GenericError)));
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
                return BadRequest(ApiResponse<TenantDTO>.Error("Ocorreu um erro interno ao processar a operaÃ§Ã£o."));
            }
        }

        [HttpGet("resolve")]
        [AllowAnonymous]
        public async Task<IActionResult> ResolveTenant([FromQuery] string subdomain)
        {
            if (string.IsNullOrEmpty(subdomain))
                return BadRequest(ApiResponse<TenantDTO>.Error("SubdomÃ­nio nÃ£o informado."));

            try
            {
                var tenant = await _tenantService.GetTenantBySubdomainAsync(subdomain);
                if (tenant == null)
                    return NotFound(ApiResponse<TenantDTO>.Error("Empresa nÃ£o encontrada."));
                    
                if (!tenant.IsActive)
                    return BadRequest(ApiResponse<TenantDTO>.Error("Conta suspensa. Contate o suporte."));

                // Retorna apenas dados pÃºblicos/inofensivos necessÃ¡rios pro frontend (Logo, Nome, Tema, ID)
                return Ok(ApiResponse<TenantDTO>.Ok(tenant, "Tenant resolvido."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<TenantDTO>.Error("Erro interno ao resolver inquilino."));
            }
        }

        [HttpPatch("{id}/toggle-status")]
        public async Task<IActionResult> ToggleStatus(Guid id)
        {
            try
            {
                var success = await _tenantService.ToggleTenantStatusAsync(id);
                return Ok(ApiResponse<object>.Ok(null, "Status alterado com sucesso."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Error("Ocorreu um erro interno ao processar a operaÃ§Ã£o."));
            }
        }
    }
}



