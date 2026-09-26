using System;

namespace Kepas.Core.Api.Domain.DTOs.Responses
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string TokenType { get; set; } = "Bearer";
        public string ErrorMessage { get; set; }
        public Guid? UserId { get; set; }
        public Guid? TenantId { get; set; }
        public string Role { get; set; }
    }
}
