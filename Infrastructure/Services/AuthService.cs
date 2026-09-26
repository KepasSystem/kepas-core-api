using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Kepas.Core.Api.Domain.Entities;
using Kepas.Core.Api.Domain.Interfaces;
using Kepas.Core.Api.Domain.DTOs.Responses;
using Kepas.Core.Api.Domain.DTOs.Requests;
using Kepas.Core.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;

namespace Kepas.Core.Api.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<AuthResult> LoginAsync(string email, string password)
        {
            var user = await _context.TenantUsers.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return new AuthResult { Success = false, ErrorMessage = "Credenciais inválidas" };
            }

            if (!user.IsActive)
                return new AuthResult { Success = false, ErrorMessage = "Usuário inativo" };

            var token = GenerateJwtToken(user.Id, user.TenantId, user.Role.Name);
            
            return new AuthResult 
            { 
                Success = true, 
                Token = token, 
                UserId = user.Id, 
                TenantId = user.TenantId,
                Role = user.Role.Name
            };
        }

        public async Task<AuthResult> LoginSuperAdminAsync(string email, string password)
        {
            var admin = await _context.PlatformAdmins.Include(a => a.PlatformRole).FirstOrDefaultAsync(a => a.Email == email);
            if (admin == null || !BCrypt.Net.BCrypt.Verify(password, admin.PasswordHash))
                return new AuthResult { Success = false, ErrorMessage = "Credenciais inv�lidas" };

            if (!admin.IsActive)
                return new AuthResult { Success = false, ErrorMessage = "Conta bloqueada" };

            var token = GenerateJwtToken(admin.Id, Guid.Empty, admin.PlatformRole?.Name ?? "Admin");

            return new AuthResult { Success = true, Token = token, UserId = admin.Id, Role = admin.PlatformRole?.Name ?? "Admin" };
        }

        public async Task<AuthResult> TenantLoginAsync(TenantLoginRequest request)
        {
            var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.Subdomain.ToLower() == request.Subdomain.ToLower());
            if (tenant == null)
                return new AuthResult { Success = false, ErrorMessage = "Workspace não encontrado" };

            var user = await _context.TenantUsers.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == request.Email && u.TenantId == tenant.Id);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return new AuthResult { Success = false, ErrorMessage = "Credenciais inválidas para este workspace" };
            }

            if (!user.IsActive || !tenant.IsActive)
                return new AuthResult { Success = false, ErrorMessage = "Conta inativa ou suspensa" };

            var token = GenerateJwtToken(user.Id, user.TenantId, user.Role.Name);
            
            return new AuthResult 
            { 
                Success = true, 
                Token = token, 
                UserId = user.Id, 
                TenantId = user.TenantId,
                Role = user.Role.Name
            };
        }

        private string GenerateJwtToken(Guid userId, Guid tenantId, string role)
        {
            var jwtKey = _config["JwtSettings:Secret"] ?? "KEPAS_VERY_LONG_SECRET_KEY_FOR_JWT_SIGNATURE_12345!";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(ClaimTypes.Role, role),
                new Claim("TenantId", tenantId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

