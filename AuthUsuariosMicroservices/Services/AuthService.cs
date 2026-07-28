using JwtClaim = System.Security.Claims.Claim;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthUsuariosMicroservice.Data;
using AuthUsuariosMicroservice.DTOs;
using AuthUsuariosMicroservice.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthUsuariosMicroservice.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> LoginAsync(LoginDto loginDto);
        string GenerarTokenJwt(Usuario usuario);
    }

    public class AuthService : IAuthService
    {
        private readonly AuthDbContext _context;
        private readonly IPasswordHasherService _hasherService;
        private readonly IConfiguration _configuration;

        public AuthService(AuthDbContext context, IPasswordHasherService hasherService, IConfiguration configuration)
        {
            _context = context;
            _hasherService = hasherService;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email.ToLower() == loginDto.Email.ToLower() && u.Estado);

            if (usuario == null) return null;

            if (!_hasherService.VerificarPasswordHash(loginDto.Password, usuario.PasswordHash, usuario.PasswordSalt))
                return null;

            var token = GenerarTokenJwt(usuario);

            return new AuthResponseDto
            {
                IdUsuario = usuario.IdUsuario,
                NombreCompleto = $"{usuario.Nombre} {usuario.Apellido}",
                Email = usuario.Email,
                Rol = usuario.Rol,
                Token = token
            };
        }

        public string GenerarTokenJwt(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = _configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("Falta Jwt:SecretKey");
            var key = Encoding.UTF8.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new JwtClaim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                    new JwtClaim(ClaimTypes.Email, usuario.Email),
                    new JwtClaim(ClaimTypes.Name, $"{usuario.Nombre} {usuario.Apellido}"),
                    new JwtClaim(ClaimTypes.Role, usuario.Rol)
                }),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}