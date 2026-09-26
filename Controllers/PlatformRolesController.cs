using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Kepas.Core.Api.Domain.Interfaces;
using Kepas.Core.Api.Domain.DTOs.Requests.Platform;
using Kepas.Core.Api.Domain.DTOs.Responses;

namespace Kepas.Core.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PlatformRolesController : ControllerBase
    {
        private readonly IPlatformRoleService _roleService;

        public PlatformRolesController(IPlatformRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(ApiResponse<object>.Ok(roles, "Cargos carregados com sucesso."));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePlatformRoleRequest request)
        {
            var role = await _roleService.CreateRoleAsync(request);
            return Ok(ApiResponse<object>.Ok(role, "Cargo criado com sucesso."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _roleService.DeleteRoleAsync(id);
            if (!result) return NotFound(ApiResponse<object>.Error("Cargo não encontrado."));
            return Ok(ApiResponse<object>.Ok(null, "Cargo removido com sucesso."));
        }
    }
}
