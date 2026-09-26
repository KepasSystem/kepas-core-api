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
    public class PlatformAdminsController : ControllerBase
    {
        private readonly IPlatformAdminService _adminService;

        public PlatformAdminsController(IPlatformAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var admins = await _adminService.GetAllAdminsAsync();
            return Ok(ApiResponse<object>.Ok(admins, "Administradores carregados com sucesso."));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePlatformAdminRequest request)
        {
            var admin = await _adminService.CreateAdminAsync(request);
            return Ok(ApiResponse<object>.Ok(admin, "Administrador criado com sucesso."));
        }

        [HttpPatch("{id}/toggle-status")]
        public async Task<IActionResult> ToggleStatus(Guid id)
        {
            var result = await _adminService.ToggleAdminStatusAsync(id);
            if (!result) return NotFound(ApiResponse<object>.Error("Administrador não encontrado."));
            return Ok(ApiResponse<object>.Ok(null, "Status alterado com sucesso."));
        }
    }
}
