using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Kepas.Core.Api.Domain.Interfaces;
using Kepas.Core.Api.Domain.DTOs.Requests.ServiceAccounts;
using Kepas.Core.Api.Domain.DTOs.Responses;

namespace Kepas.Core.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize(Roles = "Super Administrador")]
    public class ServiceAccountsController : ControllerBase
    {
        private readonly IServiceAccountService _serviceAccountService;

        public ServiceAccountsController(IServiceAccountService serviceAccountService)
        {
            _serviceAccountService = serviceAccountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var accounts = await _serviceAccountService.GetAllAsync();
            return Ok(ApiResponse<object>.Ok(accounts, "Contas de serviÃ§o carregadas com sucesso."));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateServiceAccountRequest request)
        {
            try
            {
                var account = await _serviceAccountService.CreateAsync(request);
                return Ok(ApiResponse<object>.Ok(account, "Conta de serviÃ§o criada com sucesso."));
            }
            catch (System.Exception ex)
            {
                return BadRequest(ApiResponse<object>.Error(ex.Message));
            }
        }
    }
}

