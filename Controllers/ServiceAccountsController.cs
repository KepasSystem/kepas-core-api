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
        private readonly ITranslationService _translator;

        public ServiceAccountsController(IServiceAccountService serviceAccountService, ITranslationService translator)
        {
            _serviceAccountService = serviceAccountService;
            _translator = translator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string search = "", [FromQuery] int page = 1, [FromQuery] int limit = 10)
        {
            var accounts = await _serviceAccountService.GetAllAsync(search, page, limit);
            return Ok(ApiResponse<Kepas.Core.Api.Domain.DTOs.Responses.Pagination.PagedResult<Kepas.Core.Api.Domain.DTOs.Responses.ServiceAccounts.ServiceAccountDTO>>.Ok(accounts, _translator.GetString(Kepas.Core.Api.Domain.Constants.TranslationKeys.ServiceAccountsLoadedSuccess)));
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



