using System.Threading.Tasks;
using AuthUsuariosMicroservice.DTOs;
using AuthUsuariosMicroservice.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthUsuariosMicroservice.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            if (result == null)
                return Unauthorized(new { message = "Credenciales incorrectas o usuario inactivo." });

            return Ok(result);
        }
    }
}