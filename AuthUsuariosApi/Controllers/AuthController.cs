using AuthMicroservice.DTOs;
using AuthMicroservice.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthMicroservice.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.AuthenticateAsync(loginDto);

        if (result == null)
            return Unauthorized(new { message = "Credenciales incorrectas o usuario inactivo." });

        return Ok(result);
    }
}