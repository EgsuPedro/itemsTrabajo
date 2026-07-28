using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthMicroservice.Data;
using AuthMicroservice.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


namespace AuthMicroservice.Services;

public interface IAuthService
{
    Task<AuthResponseDto?> AuthenticateAsync(LoginDto loginDto);
}

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasherService _passwordHasher;
    private readonly IConfiguration _configuration;

    public AuthService(
        ApplicationDbContext context,
        IPasswordHasherService passwordHasher,
        IConfiguration configuration)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto?> AuthenticateAsync(LoginDto loginDto)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.Estado == true);

        if (usuario == null)
            return null;

        var isPasswordValid = _passwordHasher.VerifyPasswordHash(
            loginDto.Password,
            usuario.PasswordHash,
            usuario.PasswordSalt
        );

        if (!isPasswordValid)
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]!);
        var durationMinutes = double.Parse(_configuration["JwtSettings:DurationInMinutes"] ?? "60");
        var expiration = DateTime.UtcNow.AddMinutes(durationMinutes);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, $"{usuario.IdUsuario}".ToString()),
                new Claim(ClaimTypes.Email, $"{usuario.Email}"),
                new Claim(ClaimTypes.Name, $"{usuario.Nombre} {usuario.Apellido}"),
                new Claim(ClaimTypes.Role, $"{usuario.Rol}")
            }),
            Expires = expiration,
            Issuer = _configuration["JwtSettings:Issuer"],
            Audience = _configuration["JwtSettings:Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new AuthResponseDto(
            usuario.IdUsuario,
            $"{usuario.Nombre} {usuario.Apellido}",
            usuario.Email,
            usuario.Rol,
            tokenHandler.WriteToken(token),
            expiration
        );
    }
}