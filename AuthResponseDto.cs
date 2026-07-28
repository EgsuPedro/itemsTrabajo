namespace AuthMicroservice.DTOs;

public record LoginDto(string Email, string Password);

public record AuthResponseDto(
    int IdUsuario,
    string NombreCompleto,
    string Email,
    string Rol,
    string Token,
    DateTime Expiracion
);