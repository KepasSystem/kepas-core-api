using System.Threading.Tasks;
using Kepas.Core.Api.Domain.Interfaces;
using Kepas.Core.Api.Domain.DTOs.Requests;
using Kepas.Core.Api.Domain.DTOs.Responses;
using Kepas.Core.Api.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Kepas.Core.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public AuthController(IAuthService authService, IStringLocalizer<SharedResource> localizer)
        {
            _authService = authService;
            _localizer = localizer;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request.Email, request.Password);
            if (!result.Success)
                return Unauthorized(ApiResponse<AuthResult>.Error(_localizer[nameof(ResponseCode.INVALID_CREDENTIALS)], nameof(ResponseCode.INVALID_CREDENTIALS)));

            return Ok(ApiResponse<AuthResult>.Ok(result, _localizer[nameof(ResponseCode.LOGIN_SUCCESSFUL)], nameof(ResponseCode.LOGIN_SUCCESSFUL)));
        }

        [HttpPost("tenant/login")]
        public async Task<IActionResult> TenantLogin([FromBody] TenantLoginRequest request)
        {
            var result = await _authService.TenantLoginAsync(request);
            if (!result.Success)
                return Unauthorized(ApiResponse<AuthResult>.Error(_localizer[nameof(ResponseCode.INVALID_CREDENTIALS)], nameof(ResponseCode.INVALID_CREDENTIALS)));

            return Ok(ApiResponse<AuthResult>.Ok(result, _localizer[nameof(ResponseCode.LOGIN_SUCCESSFUL)], nameof(ResponseCode.LOGIN_SUCCESSFUL)));
        }

        [HttpPost("superadmin/login")]
        public async Task<IActionResult> SuperAdminLogin([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginSuperAdminAsync(request.Email, request.Password);
            if (!result.Success)
                return Unauthorized(ApiResponse<AuthResult>.Error(_localizer[nameof(ResponseCode.INVALID_CREDENTIALS)], nameof(ResponseCode.INVALID_CREDENTIALS)));

            return Ok(ApiResponse<AuthResult>.Ok(result, _localizer[nameof(ResponseCode.LOGIN_SUCCESSFUL)], nameof(ResponseCode.LOGIN_SUCCESSFUL)));
        }
    }
}
